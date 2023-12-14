namespace Orc.LicenseManager;

using System.Collections.Generic;

public class NetworkValidationResult
{
    public NetworkValidationResult()
    {
        CurrentUsers = new List<NetworkLicenseUsage>();
    }

    public int MaximumConcurrentUsers { get; set; }

    public List<NetworkLicenseUsage> CurrentUsers { get; private set; }

    public bool IsValid
    {
        get { return CurrentUsers.Count <= MaximumConcurrentUsers; }
    }

    public override string ToString()
    {
        return string.Format("'{0}' of '{1}' current usages, license is {2}", CurrentUsers.Count, MaximumConcurrentUsers, IsValid ? "valid" : "invalid");
    }
}