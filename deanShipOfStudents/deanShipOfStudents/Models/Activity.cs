using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace deanShipOfStudents.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        [Display(Name = "Activity Name"), Required]
        public string ActivityName { get; set; }
        [Display(Name = "Location"), Required]
        public string Location { get; set; }
        [Display(Name = "Activity Type")]
        public ActivityType  type { get; set; }

        [Display(Name = "is Activity Closed?")]
        public bool IsClosed { get; set; }
        [Display(Name = "Assign Trainer")]
        public bool IsAssigned { get;  set; }
        public enum ActivityType
        {
            training_Course, WorkShop, Event, Competation, trips
        }

        public ICollection<CenterActivities> CentersActivities { get; set; }
        public ICollection<TrainerActivities> TrainerActivities { get; set; }
        public ICollection<TraineeActivites> TraineeActivities { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
        public ICollection<Matrial> Matrials { get; set; }
        public ICollection<Suggestion> suggestions { get; set; }

    }
}