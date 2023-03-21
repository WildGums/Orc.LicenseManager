namespace Orc.LicenseManager;

using Catel.ComponentModel;

public enum LicenseMode
{
    [DisplayName("CurrentUser")]
    CurrentUser,

    [DisplayName("AllUsers")]
    MachineWide
}
