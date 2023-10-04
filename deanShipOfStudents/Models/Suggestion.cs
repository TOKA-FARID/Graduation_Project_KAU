using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class Suggestion
    {
        [Key]
        public int Id { get; set; }
        [Required,Display(Name ="Your Suggestion")]
        public string text { get; set; }

        public int activityId { get; set; }

        public Activity activity { get; set; }
    }
}