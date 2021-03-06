﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkLicenseUsage.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using System.Globalization;

    public class NetworkLicenseUsage
    {
        private const string EncryptionPrefix = "_enc_";
        private const string EncryptionKey = "D274EB19-DD69-4A52-8EAF-AC2159F4D895";

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
            var message = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", Splitter, ComputerId, LicenseSignature, StartDateTime.ToString(DateTimeFormat), UserName, Ip);
            var encrypted = CryptoHelper.Encrypt(message, EncryptionKey);

            var finalMessage = $"{EncryptionPrefix}{encrypted}";
            return finalMessage;
        }

        public static NetworkLicenseUsage Parse(string text)
        {
            if (text.StartsWith(EncryptionPrefix))
            {
                var encryptedText = text.Substring(EncryptionPrefix.Length);
                text = CryptoHelper.Decrypt(encryptedText, EncryptionKey);
            }

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
