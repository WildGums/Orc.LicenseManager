// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkValidationResult.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Models
{
    using System.Collections.Generic;

    public class NetworkValidationResult
    {
        #region Constructors
        public NetworkValidationResult()
        {
            CurrentUsers = new List<NetworkLicenseUsage>();
        }
        #endregion

        #region Properties
        public int MaximumConcurrentUsers { get; set; }

        public List<NetworkLicenseUsage> CurrentUsers { get; private set; }

        public bool IsValid
        {
            get { return CurrentUsers.Count <= MaximumConcurrentUsers; }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("'{0}' of '{1}' current usages, license is {2}", CurrentUsers.Count, MaximumConcurrentUsers, IsValid ? "valid" : "invalid");
        }
    }
}