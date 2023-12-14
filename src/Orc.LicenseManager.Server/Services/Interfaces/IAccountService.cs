namespace Orc.LicenseManager.Server.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAccountService
{
    void CreateUserWithRoles(string userName, string password, List<string> userRoles);
    bool RoleExists(string rolestr);
    void CreateRole(string role);

    Task<bool> ResetPasswordAsync(string userName, string newPassword);
}