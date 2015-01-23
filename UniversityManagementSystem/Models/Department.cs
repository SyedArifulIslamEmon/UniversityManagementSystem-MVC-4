using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityManagementSystem.Models
{
    public class Department
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "The Department Code field is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Department Code must be 3 characters long")]
        [Remote("CheckCode", "Department", ErrorMessage = "This code already exist in your system")]
        [Display(Name = "Department Code")]
        public string Code { set; get; }

        [Required(ErrorMessage = "The Department Name field is required")]
        [Remote("CheckName", "Department", ErrorMessage = "This Name already exist in your system")]
        [Display(Name = "Department Name")]
        public string Name { set; get; }
    }
}