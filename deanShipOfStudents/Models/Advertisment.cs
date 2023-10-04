using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class Advertisment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Title"),Required]
        public string title { get; set; }
        [Display(Name = "Description")]

        public string body { get; set; }
        [Display(Name = "Attatchment File"),DataType(dataType:DataType.Upload)]

        public string Attatchment { get; set; }

    }
}