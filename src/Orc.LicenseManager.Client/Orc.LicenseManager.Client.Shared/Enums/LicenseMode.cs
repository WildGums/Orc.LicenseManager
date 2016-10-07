// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseType.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.ComponentModel;

    public enum LicenseMode
    {
        [Description("Current user")]
        CurrentUser,

        [Description("All users")]
        MachineWide
    }
}