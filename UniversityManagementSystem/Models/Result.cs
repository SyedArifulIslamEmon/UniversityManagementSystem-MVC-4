using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class Result
    {
        public int ResultId { set; get; }
        public virtual Student Student { set; get; }
        [Required(ErrorMessage = "The Student Registration No field is required")]
        public int StudentId { set; get; }
        public virtual Course Course { set; get; }
        [Required(ErrorMessage = "The Course field is required")]
        public int CourseId { set; get; }
        public virtual Grade Grade { set; get; }
        [Required(ErrorMessage = "The Grade Letter field is required")]
        public int GradeId { get; set; }
    }
}