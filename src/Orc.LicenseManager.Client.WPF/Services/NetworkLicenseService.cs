// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Management;
    using System.Net;
    using System.Net.Sockets;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using System.Text;
    using Models;
    using System.Collections.Generic;

    public class NetworkLicenseService : INetworkLicenseService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const int Port = 1900;

        private readonly ILicenseService _licenseService;

        private readonly System.Timers.Timer _pollingTimer = new System.Timers.Timer();

        private bool _initialized;
        private readonly List<Thread> _listeningThreads = new List<Thread>();
        private string _computerId;
        private string _userName;
        private readonly DateTime _startDateTime = DateTime.Now;

        public NetworkLicenseService(ILicenseService licenseService)
        {
            Argument.IsNotNull(() => licenseService);

            _licenseService = licenseService;

            SearchTimeout = TimeSpan.FromSeconds(2);
        }

        /// <summary>
        /// Gets the computer identifier.
        /// </summary>
        /// <value>The computer identifier.</value>
        public string ComputerId { get { return _computerId; } }

        /// <summary>
        /// Gets or sets the search timeout for other licenses on the network.
        /// </summary>
        /// <value>The search timeout.</value>
        public TimeSpan SearchTimeout { get; set; }

        /// <summary>
        /// Gets the polling interval on how often to check the network.
        /// </summary>
        /// <value>The search timeout.</value>
        public TimeSpan PollingInterval { get { return TimeSpan.FromMilliseconds(_pollingTimer.Interval); } }

        /// <summary>
        /// Occurs every time when the network validation has finished.
        /// </summary>
        public event EventHandler<NetworkValidatedEventArgs> Validated;

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <param name="pollingInterval">The polling interval. If <c>default(TimeSpan)</c>, no polling will be enabled.</param>
        /// <returns>Task.</returns>
        /// <remarks>Note that this method is optional but will start the service. If this method is not called, the service will be initialized
        /// in the <see cref="ValidateLicense" /> method.</remarks>
        public async Task Initialize(TimeSpan pollingInterval = default(TimeSpan))
        {
            await CreateLicenseListeningSockets();

            if (_pollingTimer.Enabled)
            {
                Log.Debug("Stopping network polling");

                _pollingTimer.Stop();
                _pollingTimer.Elapsed -= OnPollingTimerElapsed;
            }

            if (pollingInterval != default(TimeSpan))
            {
                if (pollingInterval < SearchTimeout)
                {
                    Log.Warning("Polling interval is smaller than SearchTimeout, defaulting to SearchTimeout + 5 seconds");

                    pollingInterval = SearchTimeout.Add(TimeSpan.FromSeconds(5));
                }

                Log.Debug("Starting network polling with an interval of '{0}'", pollingInterval);

                _pollingTimer.Interval = pollingInterval.TotalMilliseconds;
                _pollingTimer.Elapsed += OnPollingTimerElapsed;
                _pollingTimer.Start();
            }
        }

        public async Task<NetworkValidationResult> ValidateLicense()
        {
            var networkValidationResult = new NetworkValidationResult();

            var license = _licenseService.CurrentLicense;
            if (license == null)
            {
                return networkValidationResult;
            }

            networkValidationResult.MaximumConcurrentUsers = license.GetMaximumConcurrentLicenses();

            Log.Info("Checking for other licenses, maximum number of concurrent users allowed is '{0}'", networkValidationResult.MaximumConcurrentUsers);

            try
            {
                await CreateLicenseListeningSockets();

                var timeout = SearchTimeout != null ? (int)SearchTimeout.TotalMilliseconds : 2000;

                var licenseUsages = new List<NetworkLicenseUsage>();
                var tasks = new List<Task<List<NetworkLicenseUsage>>>();

                foreach (var ipAddress in GetIpAddresses())
                {
                    tasks.Add(BroadcastMessage(ipAddress, license.Signature, timeout));
                }

                await Task.Factory.StartNew(() => Task.WaitAll(tasks.ToArray()));

                foreach (var task in tasks)
                {
                    licenseUsages.AddRange(task.Result);
                }

                networkValidationResult.CurrentUsers.AddRange(licenseUsages.GroupBy(x => x.ComputerId).Select(group => group.First()));

                Log.Debug("Found {0}", networkValidationResult);

                Validated.SafeInvoke(this, new NetworkValidatedEventArgs(networkValidationResult));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to check for maximum number of concurrent users");
            }

            return networkValidationResult;
        }

        private async void OnPollingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await ValidateLicense();
        }

        private async Task CreateLicenseListeningSockets()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            if (string.IsNullOrEmpty(_userName))
            {
                _userName = Environment.UserName;
            }

            if (string.IsNullOrEmpty(_computerId))
            {
                _computerId = await Task.Factory.StartNew(() => GetComputerId());
            }

            Log.Debug("Creating local license service and registering license sockets on local network");

            var ipAddresses = GetIpAddresses();

            foreach (var ipAddress in ipAddresses)
            {
                var thread = new Thread(HandleIncomingRequests);
                thread.IsBackground = true;
                thread.Start(ipAddress);

                _listeningThreads.Add(thread);
            }
        }

        private async Task<List<NetworkLicenseUsage>> BroadcastMessage(string ipAddress, string message, int maxTimeout = 1000)
        {
            var licenseUsages = new Dictionary<string, NetworkLicenseUsage>();

            Log.Debug("Broadcasting via ip '{0}' to see how much users are currently using the license", ipAddress);

            try
            {
                using (var udpClient = new UdpClient())
                {
                    udpClient.Client.ReceiveTimeout = maxTimeout;
                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    udpClient.EnableBroadcast = true;

                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), 0));

                    var sendBuffer = Encoding.ASCII.GetBytes(message);

                    var remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, Port);
                    udpClient.Send(sendBuffer, sendBuffer.Length, remoteEndPoint);

                    await Task.Factory.StartNew(() =>
                    {
                        var endDateTime = DateTime.Now.AddMilliseconds(maxTimeout);

                        while (endDateTime >= DateTime.Now)
                        {
                            try
                            {
                                var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                                var receiveBuffer = udpClient.Receive(ref ipEndPoint);
                                if (receiveBuffer != null && receiveBuffer.Length > 0)
                                {
                                    var receivedMessage = Encoding.ASCII.GetString(receiveBuffer);

                                    Log.Debug("Received message '{0}' from '{1}'", receivedMessage, ipEndPoint.Address);

                                    var licenseUsage = NetworkLicenseUsage.Parse(receivedMessage);
                                    if (licenseUsage == null)
                                    {
                                        continue;
                                    }

                                    if (string.Equals(licenseUsage.LicenseSignature, message))
                                    {
                                        Log.Debug("Received message from '{0}' that license is being used", ipEndPoint.Address);

                                        var computerId = licenseUsage.ComputerId;

                                        var add = true;
                                        if (licenseUsages.ContainsKey(computerId))
                                        {
                                            // Only update if this is an older timestamp
                                            if (licenseUsages[computerId].StartDateTime < licenseUsage.StartDateTime)
                                            {
                                                add = false;
                                            }
                                        }

                                        if (add)
                                        {
                                            licenseUsages[computerId] = licenseUsage;
                                        }
                                    }
                                }
                            }
                            catch (SocketException)
                            {
                                // ignore
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to broadcast message, defaulting to 0 currently active licences");
            }

            return licenseUsages.Values.ToList();
        }

        private async void HandleIncomingRequests(object ipAddressAsObject)
        {
            try
            {
                var ipAddress = (ipAddressAsObject != null) ? IPAddress.Parse((string)ipAddressAsObject) : IPAddress.Any;

                Log.Debug("Creating listener for ip '{0}'", ipAddress);

                var udpClient = new UdpClient();

                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.ExclusiveAddressUse = false;
                udpClient.EnableBroadcast = true;

                udpClient.Client.Bind(new IPEndPoint(ipAddress, Port));

                var licenseSignature = string.Empty;

                while (true)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(licenseSignature))
                        {
                            var currentLicense = _licenseService.CurrentLicense;
                            if (currentLicense != null)
                            {
                                licenseSignature = currentLicense.Signature;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(licenseSignature))
                        {
                            // No reason to wait for something, wait and continue
                            Thread.Sleep(5000);
                            continue;
                        }

                        var ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
                        var data = udpClient.Receive(ref ipEndPoint);

                        var message = Encoding.ASCII.GetString(data);
                        if (string.Equals(message, licenseSignature))
                        {
                            Log.Debug("Received request from '{0}' on '{1}' to get currently used license", ipEndPoint.Address, udpClient.Client.LocalEndPoint);

                            var licenseUsage = new NetworkLicenseUsage(_computerId, ipAddress.ToString(), _userName, licenseSignature, _startDateTime);

                            var responseMessage = licenseUsage.ToNetworkMessage();
                            var responseBytes = ASCIIEncoding.ASCII.GetBytes(responseMessage);

                            udpClient.Send(responseBytes, responseBytes.Length, ipEndPoint);
                        }
                    }
                    catch (SocketException)
                    {
                        // Ignore, it's probably the timeout
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to handle incoming requests, probably a process is already running on the same port");
            }
        }

        private string GetComputerId()
        {
            Log.Debug("Retrieving CPU id");

            var cpuId = string.Empty;

            try
            {
                var managementClass = new ManagementClass("win32_processor");
                var managementObjectCollection = managementClass.GetInstances();

                foreach (var managementObject in managementObjectCollection)
                {
                    cpuId = managementObject.Properties["processorID"].Value.ToString();
                    break;
                }

                Log.Debug("Retrieved CPU id '{0}'", cpuId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve CPU id");
            }

            return CalculateMd5Hash(cpuId);
        }

        private string CalculateMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private List<string> GetIpAddresses()
        {
            var selfIps = new List<string>();

            var hostName = Dns.GetHostName();
            var host = Dns.GetHostEntry(hostName);

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (!ip.ToString().StartsWith("169"))
                    {
                        selfIps.Add(ip.ToString());
                    }
                }
            }

            return selfIps;
        }
    }
}