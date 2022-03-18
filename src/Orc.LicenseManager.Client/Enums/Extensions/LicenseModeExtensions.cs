// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseModeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.ComponentModel;
    using System.Linq;

    public static class LicenseModeExtensions
    {
        public static LicenseMode ToOpposite(this LicenseMode licenseMode)
        {
            if (licenseMode == LicenseMode.CurrentUser)
            {
                return LicenseMode.MachineWide;
            }

            return LicenseMode.CurrentUser;
        }

        public static string ToDescriptionText(this LicenseMode licenseMode)
        {
            var descriptionAttribute = typeof(LicenseMode)
                .GetField(licenseMode.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute is not null ? descriptionAttribute.Description
                : licenseMode.ToString();
        }
    }
}