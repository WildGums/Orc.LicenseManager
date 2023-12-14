namespace Orc.LicenseManager.Server.Data;

using System.Collections.Generic;
using System.Data.Entity;
using Services;

public class RecreateContext : DropCreateDatabaseAlways<LicenseManagerDbContext>
{
    private readonly IAccountService _accountService;

    public RecreateContext(IAccountService accountService)
    {
        _accountService = accountService;
    }

    protected override void Seed(LicenseManagerDbContext context)
    {
        _accountService.CreateRole("Admin");
        _accountService.CreateUserWithRoles("Admin", "password123", new List<string>
        {
            "Admin"
        });
        base.Seed(context);
    }
}
