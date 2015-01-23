using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class RoomAllocation
    {
        public int RoomAllocationId { set; get; }
        public virtual Department Department { set; get; }

        [Required(ErrorMessage = "The Department field is required")]
        public int DepartmentId { set; get; }

        public virtual Course Course { set; get; }

        [Required(ErrorMessage = "The Course field is required")]
        public int CourseId { set; get; }
        public virtual Room Room { set; get; }

        [Required(ErrorMessage = "The Room No field is required")]
        public int RoomId { set; get; }

        public virtual Day Day { set; get; }

        [Required(ErrorMessage = "The Day field is required")]
        public int DayId { set; get; }

        [Required(ErrorMessage = "The From field is required")]
        public double StartTime { set; get; }

        [Required(ErrorMessage = "The To field is required")]
        public double EndTime { set; get; }

    }
}