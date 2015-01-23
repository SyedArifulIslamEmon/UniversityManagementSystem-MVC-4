using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityManagementSystem.Models
{
    public class Course
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "The Course Code field is required")]
        [Remote("CheckCode", "Course", ErrorMessage = "This code already exist in your system")]
        [Display(Name = "Course Code")]
        public string Code { set; get; }
        [Required(ErrorMessage = "The Course Name field is required")]
        [Remote("CheckName", "Course", ErrorMessage = "This Name already exist in your system")]
        [Display(Name = "Course Name")]
        public string Name { set; get; }
        [Required(ErrorMessage = "The Course Description field is required")]
        [Display(Name = "Course Description")]
        public string Description { set; get; }
        
        public virtual Department Deparment { set; get; }

        [Required(ErrorMessage = "The Department field is required")]
        public int DepartmentId { set; get; }

        public virtual Semester Semester { set; get; }

        [Required(ErrorMessage = "The Semester field is required")]
        public int SemesterId { set; get; }
        
        [Required(ErrorMessage = "The Course Credit field is required")]
        [Display(Name = "Course Credit")]
        [Range(typeof(double), "0.00", "2147483647.00", ErrorMessage = "Credit Should be a positive number")]
        public double Credit { set; get; }

    }
}