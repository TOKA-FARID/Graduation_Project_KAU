using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class Matrial
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Matrial Name")]

        public string matrialName { get; set; }
        [Display(Name ="File type")]
        public string ext { get;  set; }
        [DataType(DataType.Upload),Display(Name ="File name")]
        public string filePath { get; set; }

        [ForeignKey("activity"),Display(Name = "activity")]
        public int activityId { get; set; }
        public Activity activity { get; set; }
    }
}