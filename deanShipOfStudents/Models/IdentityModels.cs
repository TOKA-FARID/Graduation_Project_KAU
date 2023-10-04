using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace deanShipOfStudents.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Middle name")]
        public string middleName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string UniverstyId { get; set; }

        public UserType userType { get; set; }
        public bool isActive { get; set; }

        public ICollection<Center> Centers { get; set; }
        public ICollection<TrainerActivities> TrainerActivities { get; set; }
        public ICollection<TraineeActivites> TraineeActivities { get; set; }

        public enum UserType
        {
            admin,
            superVisor,
            trainer,
            trainee
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Center> Centers { get; set; }
        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Activity> Activities { get; set; }
        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Meeting> Meetings { get; set; }
        public System.Data.Entity.DbSet<deanShipOfStudents.Models.TrainerActivities> TrainerActivities { get; set; }
        public System.Data.Entity.DbSet<deanShipOfStudents.Models.TraineeActivites> TraineeActivities { get; set; }
        public System.Data.Entity.DbSet<deanShipOfStudents.Models.CenterActivities> CenterActivities { get; set; }

        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Matrial> Matrials { get; set; }

        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Attatchment> Attatchments { get; set; }

        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Advertisment> Advertisments { get; set; }

        public System.Data.Entity.DbSet<deanShipOfStudents.Models.Suggestion> Suggestions { get; set; }

        public System.Data.Entity.DbSet<deanShipOfStudents.Models.News> News { get; set; }
    }
}