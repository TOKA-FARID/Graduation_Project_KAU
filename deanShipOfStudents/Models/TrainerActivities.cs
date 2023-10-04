using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class TrainerActivities
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Trainer"), Column(Order = 2)]
        public string TrainerId { get; set; }
        [ForeignKey("Activity"), Column(Order = 3)]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public virtual ApplicationUser Trainer { get; set; }
    }
}