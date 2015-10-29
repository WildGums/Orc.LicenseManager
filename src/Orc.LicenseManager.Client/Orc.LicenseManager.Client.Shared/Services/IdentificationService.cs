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
    using SystemInfo;
    using Catel;
    using Catel.Logging;
    using Catel.Threading;
    using MethodTimer;

    public class IdentificationService : IIdentificationService
    {
        private readonly ISystemIdentificationService _systemIdentificationService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly object _lock = new object();
        private string _machineId = string.Empty;

        public IdentificationService(ISystemIdentificationService systemIdentificationService)
        {
            Argument.IsNotNull(() => systemIdentificationService);

            _systemIdentificationService = systemIdentificationService;
        }

        public virtual string GetMachineId()
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(_machineId))
                {
                    _machineId = CalculateMachineId();
                }

                return _machineId;
            }
        }

        [Time]
        private string CalculateMachineId()
        {
            Log.Debug("Retrieving machine id");

            var cpuId = string.Empty;
            var motherboardId = string.Empty;
            var hddId = string.Empty;
            var gpuId = string.Empty;

            TaskHelper.RunAndWait(new Action[]
            {
                    () => cpuId = "CPU >> " + _systemIdentificationService.GetCpuId(),
                    () => motherboardId = "BASE >> " + _systemIdentificationService.GetMotherboardId(),
                    () => hddId = "HDD >> " + _systemIdentificationService.GetHardDriveId(),
                    () => gpuId = "GPU >> " + _systemIdentificationService.GetGpuId(),
                //() => gpuId = "MAC >> " + _systemIdentificationService.GetMacId(),
            });

            var values = new List<string>(new[]
            {
                    cpuId,
                    motherboardId,
                    hddId,
                    gpuId
                });

            var hashedValues = new List<string>();

            foreach (var value in values)
            {
                var hashedValue = CalculateMd5Hash(value);
                hashedValues.Add(hashedValue);

                Log.Debug("* {0} => {1}", value, hashedValue);
            }

            var machineId = string.Join(LicenseElements.IdentificationSeparator, hashedValues);

            Log.Debug("Hashed machine id '{0}'", machineId);

            return machineId;
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