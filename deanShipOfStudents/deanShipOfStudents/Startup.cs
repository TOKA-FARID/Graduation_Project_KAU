using deanShipOfStudents.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(deanShipOfStudents.Startup))]
namespace deanShipOfStudents
{
    public partial class Startup
    {
        private deanShipOfStudents.Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
            CreateAdministrator();

        }
        public void CreateAdministrator()
        {
            var superAdmin = db.Users.Where(u => u.UserName.Equals("Administrator")).ToList().FirstOrDefault();
            if (superAdmin == null)
            {
                try
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var user = new ApplicationUser();
                    user.firstName = "admin";
                    user.middleName = "admin";
                    user.lastName = "admin";
                    user.UniverstyId = "1111111";
                    user.userType = ApplicationUser.UserType.admin;
                    user.UserName = "Administrator";
                    user.PhoneNumber = "055555555";
                    user.PhoneNumberConfirmed = true;
                    user.Email = "tt@tt.com";
                    user.EmailConfirmed = true;
                    user.isActive = true;
                    var check = userManager.Create(user, "Aa12345678@");
                    if (check.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Admin");
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException et)
                {
                    string msg = "";
                    foreach (var item in et.EntityValidationErrors)
                    {
                        foreach (var x in item.ValidationErrors)
                        {
                            msg += "property Name : " + x.PropertyName + " error Message " + x.ErrorMessage + " \n";
                        }
                    }

                    System.Diagnostics.Debug.WriteLine(msg);
                }catch(Exception e)
                {

                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
               
            }

        }
        public void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole role;
            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("SuperVisor"))
            {
                role = new IdentityRole();
                role.Name = "SuperVisor";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("trainer"))
            {
                role = new IdentityRole();
                role.Name = "trainer";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("trainee"))
            {
                role = new IdentityRole();
                role.Name = "trainee";
                roleManager.Create(role);
            }
        }
    }
}
