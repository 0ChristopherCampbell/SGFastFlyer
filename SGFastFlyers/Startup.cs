using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;

using System.Web;
using SGFastFlyers.DataAccessLayer;
using SGFastFlyers.Models;

[assembly: OwinStartupAttribute(typeof(SGFastFlyers.Startup))]
namespace SGFastFlyers
{
    public partial class Startup
    {
        string defaultAdminUserName = "admin@sgfastflyers.com";
        string defaultAdminPassword = "defaultpassword_1";

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ApplicationDbContext context = new ApplicationDbContext();
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // Create Admin role. 
                roleManager.Create(new IdentityRole("Admin"));
            }

            // Create default admin superuser.    
            ApplicationUser user = new ApplicationUser()
            {
                UserName = defaultAdminUserName
            };

            var chkUser = userManager.Create(user, defaultAdminPassword);

            //Add default User to Role Admin   
            if (chkUser.Succeeded)
            {
                IdentityResult result = userManager.AddToRole(user.Id, "Admin");
            }
        }        
    }
}
