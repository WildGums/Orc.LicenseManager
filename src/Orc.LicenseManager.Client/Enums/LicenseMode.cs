// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseType.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using Catel.ComponentModel;

    public enum LicenseMode
    {
        [DisplayName("CurrentUser")]
        CurrentUser,

        [DisplayName("AllUsers")]
        MachineWide
    }
}