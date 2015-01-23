using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityManagementSystem.Models
{
    public class Teacher
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "The Teacher Name field is required")]
        [Display(Name = "Teacher Name")]
        public string Name { set; get; }
        [Required(ErrorMessage = "The Teacher Address field is required")]
        [Display(Name = "Teacher Address")]
        public string Address { set; get; }
        [Required(ErrorMessage = "The Teacher Email field is required")]
        [Remote("CheckEmail", "Teacher", ErrorMessage = "This Email already exist in your system")]

        [Display(Name = "Teacher Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }
        [Required(ErrorMessage = "The Teacher Contact No field is required")]
        [Display(Name = "Teacher Contact No")]
        [RegularExpression(@"^[0-9]{1,20}$", ErrorMessage = "Only numbers are allowed.")]
        public string ContactNo { set; get; }
        public virtual Designation Designation { set; get; }
        public virtual Department Department { set; get; }

        [Required(ErrorMessage = "The Teacher Designation field is required")]
        public int DesignationId { set; get; }

        [Required(ErrorMessage = "The Teacher Department field is required")]
        public int DepartmentId { set; get; }
        [Required(ErrorMessage = "The Teacher Credit field is required")]
        [Display(Name = "Teacher Credit")]
        [Range(typeof(double), "0.00", "2147483647.00", ErrorMessage = "Credit Should be a positive number")]
        public double Credit { set; get; }
    }
}