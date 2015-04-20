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

        public string GetMachineId()
        {
            Log.Debug("Retrieving machine id");

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

                Log.Debug("Retrieved machine id '{0}'", cpuId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve machine id");
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
    }
}