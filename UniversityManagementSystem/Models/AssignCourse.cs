using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class AssignCourse
    {
        public int Id { set; get; }
        public virtual Department Department { set; get; }

        [Required(ErrorMessage = "The Department field is required")]
        public int DepartmentId { set; get; }
        public virtual Teacher Teacher { set; get; }

        [Required(ErrorMessage = "The Teacher field is required")]
        public int TeacherId { set; get; }
        public virtual Course Course { set; get; }

        [Required(ErrorMessage = "The Course field is required")]
        public int CourseId { set; get; }
    }
}