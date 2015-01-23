using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class EnrollCourse
    {
        public int EnrollCourseId { set; get; }
        public virtual Student Student { set; get; }

        [Display(Name = "Student Registration No")]
        [Required(ErrorMessage = "The Student Registration No field is required")]
        public int StudentId { set; get; }
        
        public virtual Course Course { set; get; }
        
        [Display(Name = "Select Course")]
        [Required(ErrorMessage = "The Course field is required")]
        public int CourseId { set; get; }
        
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The Date field is required")]
        public DateTime Date { set; get; }
    }
}