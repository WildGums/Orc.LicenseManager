namespace Orc.LicenseManager;

using System.ComponentModel;
using System.Linq;

public static class LicenseModeExtensions
{
    public static LicenseMode ToOpposite(this LicenseMode licenseMode)
    {
        return licenseMode == LicenseMode.CurrentUser 
            ? LicenseMode.MachineWide
            : LicenseMode.CurrentUser;
    }

    public static string ToDescriptionText(this LicenseMode licenseMode)
    {
        return typeof(LicenseMode)
            .GetField(licenseMode.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() is DescriptionAttribute descriptionAttribute ? descriptionAttribute.Description
            : licenseMode.ToString();
    }
}
