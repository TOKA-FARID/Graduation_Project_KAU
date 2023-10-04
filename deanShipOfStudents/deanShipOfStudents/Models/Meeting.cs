using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace deanShipOfStudents.Models
{
    public class Meeting
    {
        public int Id { get; set; }

        [Display(Name = "Date "), Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime DateOfActivity { get; set; }

        [Display(Name = "time "), Required(ErrorMessage = "*")]
        [DataType(DataType.Time)]
        public DateTime TimeOfActivity { get; set; }

        [Display(Name = "Activity name" )]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}