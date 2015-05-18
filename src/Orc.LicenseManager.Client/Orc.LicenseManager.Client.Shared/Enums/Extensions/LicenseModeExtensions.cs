// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseModeExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Enums.Extensions
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

            return descriptionAttribute != null
                ? descriptionAttribute.Description
                : licenseMode.ToString();
        }
    }
}