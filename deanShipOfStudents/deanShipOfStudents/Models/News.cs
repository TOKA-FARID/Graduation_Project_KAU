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
        [Display(Name ="File Name"),Required]
        public string FileName { get; set; }
        [Display(Name ="location in the page")]
        public Newstype type { get; set; }
        [Display(Name ="is Published ?")]
        public bool isPublished { get; set; }


        public enum Newstype
        { 
            slider , reguler
        }
    }
}