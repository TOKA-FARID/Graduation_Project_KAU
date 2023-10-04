using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class Attatchment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("traine") ,Display(Name ="Traine")]
        public string traineId { get; set; }

        [Display(Name = "degree")]
        public double degree { get; set; }
       
        public FileType type { get; set; }
        
        public ApplicationUser traine { get; set; }
        [ForeignKey("activity"),Display(Name = "Activity")]
        public int activityId { get; set; }
        
        public Activity activity { get; set; }
        public enum FileType
        {
            attendance,evaluation
        }
    }
}