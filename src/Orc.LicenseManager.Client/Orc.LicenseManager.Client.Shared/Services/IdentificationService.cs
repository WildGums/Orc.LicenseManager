// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentificationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Management;
    using System.Security.Cryptography;
    using System.Text;
    using Catel.Logging;

    public class IdentificationService : IIdentificationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lock = new object();
        private string _machineId = string.Empty;

        public string GetMachineId()
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(_machineId))
                {
                    return _machineId;
                }

                Log.Debug("Retrieving machine id");

                var cpuId = string.Empty;

                try
                {
                    //var managementClass = new ManagementClass("win32_processor");
                    //var managementObjectCollection = managementClass.GetInstances();

                    // Searching should be faster
                    var query = "SELECT ProcessorId FROM Win32_Processor";
                    var searcher = new ManagementObjectSearcher(query);
                    var managementObjectCollection = searcher.Get();

                    foreach (var managementObject in managementObjectCollection)
                    {
                        cpuId = managementObject.Properties["ProcessorID"].Value.ToString();
                        break;
                    }

                    Log.Debug("Retrieved machine id '{0}'", cpuId);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to retrieve machine id");
                }

                _machineId = CalculateMd5Hash(cpuId);
            }

            return _machineId;
        }

        private string CalculateMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}