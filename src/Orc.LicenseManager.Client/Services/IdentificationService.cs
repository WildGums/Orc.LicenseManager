namespace Orc.LicenseManager;

using SystemInfo;
using System;

public class IdentificationService : IIdentificationService
{
    private readonly ISystemIdentificationService _systemIdentificationService;
    private readonly object _lock = new object();
    private string _machineId = string.Empty;

    public IdentificationService(ISystemIdentificationService systemIdentificationService)
    {
        ArgumentNullException.ThrowIfNull(systemIdentificationService);

        _systemIdentificationService = systemIdentificationService;
    }

    public virtual string GetMachineId()
    {
        lock (_lock)
        {
            if (string.IsNullOrWhiteSpace(_machineId))
            {
                _machineId = _systemIdentificationService.GetMachineId(LicenseElements.IdentificationSeparator, false);
            }

            return _machineId;
        }
    }
}