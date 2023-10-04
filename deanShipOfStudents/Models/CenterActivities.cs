using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class CenterActivities
    {
        [Key]
        public int CenterActivitiesId { get; set; }
        [ForeignKey("Center"), Column(Order = 1)]
        public int CenterId { get; set; }
        [ForeignKey("Activity"), Column(Order = 2)]
        public int ActivityId { get; set; }

        public Center Center { get; set; }
        public Activity Activity { get; set; }
    }
}