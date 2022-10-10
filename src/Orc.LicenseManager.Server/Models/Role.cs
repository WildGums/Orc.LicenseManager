namespace Orc.LicenseManager.Server
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Role : IdentityRole
    {
        public Role()
        {
        }

        public Role(string rolename)
        {
            Name = rolename;
        }
    }
}
