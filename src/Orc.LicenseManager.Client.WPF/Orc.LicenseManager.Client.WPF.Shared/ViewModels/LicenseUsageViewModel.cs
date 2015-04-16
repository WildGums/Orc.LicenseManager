// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseUsageViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM;
    using Catel.Reflection;
    using Models;

    public class LicenseUsageViewModel : ViewModelBase
    {
        #region Constructors
        public LicenseUsageViewModel()
        {
            var assembly = AssemblyHelper.GetEntryAssembly();
            WindowTitle = assembly.Title() + " licence usage" ;
            CurrentUsers = new List<NetworkLicenseUsage>()
            {
                new NetworkLicenseUsage("12", "222", "Jon", "Licence signature", DateTime.Now),
                new NetworkLicenseUsage("12", "222", "Jane", "Licence signature", DateTime.Now),
                new NetworkLicenseUsage("12", "222", "Samuel", "Licence signature", DateTime.Now),
                new NetworkLicenseUsage("12", "222", "Paula", "Licence signature", DateTime.Now)
            };
        }
        #endregion

        public string WindowTitle { get; set; }
        public List<NetworkLicenseUsage> CurrentUsers{ get; set; }
    }
}