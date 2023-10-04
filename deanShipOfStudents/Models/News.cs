using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="File name"),Required]
        public string FileName { get; set; }
        [Display(Name ="Location in the page")]
        public Newstype type { get; set; }
        [Display(Name ="Is Published ?")]
        public bool isPublished { get; set; }


        public enum Newstype
        { 
            slider , reguler
        }
    }
}