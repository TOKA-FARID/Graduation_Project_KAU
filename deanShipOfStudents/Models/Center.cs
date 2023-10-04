using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class Center
    {
        [Key]
        public int CenterId { get; set; }

        [Display(Name ="Center Name"),Required]
        public string CenterName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Objective")]
        public string Objective { get; set; }

        [Display(Name = "Supervisor")]
        [ForeignKey("Supervisor")]
        public string SupervisorId { get; set; }
        public ApplicationUser Supervisor { get; set; }
        public ICollection<CenterActivities> CentersActivities { get; set; }

    }
}