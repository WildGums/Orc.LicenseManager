// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseType.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.ComponentModel;

    public enum LicenseMode
    {
        [Description("Current user license")]
        CurrentUser,

        [Description("Machine wide license")]
        MachineWide
    }
}