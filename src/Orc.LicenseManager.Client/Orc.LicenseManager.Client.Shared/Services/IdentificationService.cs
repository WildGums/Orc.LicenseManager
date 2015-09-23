// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentificationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Management;
    using System.Security.Cryptography;
    using System.Text;
    using Catel.Logging;

    public class IdentificationService : IIdentificationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lock = new object();
        private string _machineId = string.Empty;

        public virtual string GetMachineId()
        {
            lock (_lock)
            {
                if (!string.IsNullOrWhiteSpace(_machineId))
                {
                    return _machineId;
                }

                Log.Debug("Retrieving machine id");

                var values = new List<string>();
                values.Add("CPU >> " + GetCpuId());
                values.Add("BASE >> " + GetMotherboardId());
                values.Add("HDD >> " + GetHardDriveId());
                values.Add("GPU >> " + GetGpuId());
                //values.Add("MAC >> " + GetMacId());

                var hashedValues = new List<string>();

                foreach (var value in values)
                {
                    var hashedValue = CalculateMd5Hash(value);
                    hashedValues.Add(hashedValue);

                    Log.Debug("* {0} => {1}", value, hashedValue);
                }

                var machineId = string.Join("|", hashedValues);

                Log.Debug("Hashed machine id '{0}'", machineId);

                _machineId = machineId;
            }

            return _machineId;
        }

        protected virtual string GetMacId()
        {
            var identifier = "Wireless: " + GetIdentifier("Win32_NetworkAdapter", "MACAddress", "AdapterType", "Wireless")
                + "Wired: " + GetIdentifier("Win32_NetworkAdapter", "MACAddress", "AdapterType", "Ethernet 802.3");

            Log.Debug("Using mac id '{0}'", identifier);

            return identifier;
        }

        protected virtual string GetGpuId()
        {
            var identifier = GetIdentifier("Win32_VideoController", "DeviceID")
                + GetIdentifier("Win32_VideoController", "Name");

            Log.Debug("Using gpu id '{0}'", identifier);

            return identifier;
        }

        protected virtual string GetHardDriveId()
        {
            var identifier = GetIdentifier("Win32_DiskDrive", "Model", "InterfaceType", "!USB")
                + GetIdentifier("Win32_DiskDrive", "Manufacturer", "InterfaceType", "!USB")
                + GetIdentifier("Win32_DiskDrive", "Signature", "InterfaceType", "!USB")
                + GetIdentifier("Win32_DiskDrive", "TotalHeads", "InterfaceType", "!USB")
                + GetIdentifier("Win32_DiskDrive", "DeviceID", "InterfaceType", "!USB")
                + GetIdentifier("Win32_DiskDrive", "SerialNumber", "InterfaceType", "!USB");

            Log.Debug("Using hdd id '{0}'", identifier);

            return identifier;
        }

        protected virtual string GetMotherboardId()
        {
            // Note: not sure why this returns empty strings on some machines
            var identifier = GetIdentifier("Win32_ComputerSystemProduct", "IdentifyingNumber")
                + GetIdentifier("Win32_ComputerSystemProduct", "UUID");

            Log.Debug("Using motherboard id '{0}'", identifier);

            return identifier;
        }

        protected virtual string GetCpuId()
        {
            // Uses first CPU identifier available in order of preference
            var identifier = GetIdentifier("Win32_Processor", "UniqueId");
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                Log.Debug("Using Processor.UniqueId to identify cpu '{0}'", identifier);

                return identifier;
            }

            identifier = GetIdentifier("Win32_Processor", "ProcessorId");
            if (!string.IsNullOrWhiteSpace(identifier))
            {
                Log.Debug("Using Processor.ProcessorId to identify cpu '{0}'", identifier);

                return identifier;
            }

            identifier += GetIdentifier("Win32_Processor", "Name")
                + GetIdentifier("Win32_Processor", "SerialNumber")
                + GetIdentifier("Win32_Processor", "Manufacturer")
                + GetIdentifier("Win32_Processor", "NumberOfCores")
                + GetIdentifier("Win32_Processor", "NumberOfLogicalProcessors")
                + GetIdentifier("Win32_Processor", "MaxClockSpeed")
                + GetIdentifier("Win32_Processor", "Version");

            Log.Debug("Using Processor.Manufacturer + MaxClockSpeed + Version to identify cpu '{0}'", identifier);

            return identifier;
        }

        protected static string GetIdentifier(string wmiClass, string wmiProperty)
        {
            return GetIdentifier(wmiClass, wmiProperty, null, null);
        }

        protected static string GetIdentifier(string wmiClass, string wmiProperty, string additionalWmiToCheck, string additionalWmiToCheckValue)
        {
            var result = string.Empty;

            var managementClass = new ManagementClass(wmiClass);
            var managementObjectCollection = managementClass.GetInstances();
            foreach (var managementObject in managementObjectCollection)
            {
                // Only get the first one
                if (string.IsNullOrEmpty(result))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(additionalWmiToCheck))
                        {
                            var wmiToCheckValue = managementObject[additionalWmiToCheck];

                            var wmiToCheckValueValue = additionalWmiToCheckValue;
                            var invert = additionalWmiToCheckValue.StartsWith("!");
                            if (invert)
                            {
                                wmiToCheckValueValue = additionalWmiToCheckValue.Substring(1);
                            }

                            var equals = string.Equals(wmiToCheckValue.ToString(), wmiToCheckValueValue, StringComparison.OrdinalIgnoreCase);
                            if ((!equals && !invert) || (equals && invert))
                            {
                                Log.Debug("Cannot use mgmt object '{0}', wmi property '{1}' is '{2}' but expected '{3}'", wmiClass,
                                    additionalWmiToCheck, wmiToCheckValue, additionalWmiToCheckValue);
                                continue;
                            }
                        }

                        result = managementObject[wmiProperty].ToString();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex, "Failed to retrieve object '{0}.{1}', additional wmi to check: {2}", wmiClass, wmiProperty, additionalWmiToCheck);
                    }
                }
            }

            return result;
        }

        private static string CalculateMd5Hash(string input)
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