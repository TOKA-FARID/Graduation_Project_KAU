using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class TraineeActivites
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Trainee"), Column(Order = 2)]
        public string TraineeId { get; set; }
        [ForeignKey("Activity"), Column(Order = 3)]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public virtual ApplicationUser Trainee { get; set; }
    }
}