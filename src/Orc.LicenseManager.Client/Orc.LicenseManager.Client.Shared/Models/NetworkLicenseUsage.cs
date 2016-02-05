// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkLicenseUsage.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Models
{
    using System;
    using System.Globalization;

    public class NetworkLicenseUsage
    {
        private const string DateTimeFormat = "yyyyMMddHHmmss";
        private const string Splitter = "|+|";

        #region Constructors
        public NetworkLicenseUsage(string computerId, string ip, string userName, string licenseSignature, DateTime startDateTime)
        {
            ComputerId = computerId;
            Ip = ip;
            UserName = userName;
            LicenseSignature = licenseSignature;
            StartDateTime = startDateTime;
        }
        #endregion

        public string ComputerId { get; private set; }

        public string Ip { get; private set; }

        public string UserName { get; private set; }

        public string LicenseSignature { get; private set; }

        public DateTime StartDateTime { get; private set; }

        public override string ToString()
        {
            return string.Format("Id: {0} | Ip: {1} | Start time: {2}", ComputerId, Ip, StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public string ToNetworkMessage()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", Splitter, ComputerId, LicenseSignature, StartDateTime.ToString(DateTimeFormat), UserName, Ip);
        }

        public static NetworkLicenseUsage Parse(string text)
        {
            var splitted = text.Split(new[] { Splitter }, StringSplitOptions.None);

            // Backwards compatibility
            if (splitted.Length < 2)
            {
                splitted = text.Split(new[] {'|'}, StringSplitOptions.None);
            }

            var computerId = GetValue(splitted, 0);
            var licenseSignature = GetValue(splitted, 1);
            var startDateTime = DateTime.ParseExact(GetValue(splitted, 2), DateTimeFormat, CultureInfo.InvariantCulture);
            var userName = GetValue(splitted, 3);
            var ip = GetValue(splitted, 4);

            return new NetworkLicenseUsage(computerId, ip, userName, licenseSignature, startDateTime);
        }

        private static string GetValue(string[] splittedStrings, int index)
        {
            return (splittedStrings.Length >= index) ? splittedStrings[index] : string.Empty;
        }
    }
}