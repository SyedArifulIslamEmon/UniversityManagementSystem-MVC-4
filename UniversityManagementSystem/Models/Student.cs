using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "Student Registration No")]
        public virtual string RegistrationId { set; get; }

        [Display(Name="Student Name")]
        [Required(ErrorMessage = "Student Name Required!")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Required!")]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckEmail", "Student", ErrorMessage = "This Email already exist in your system")]
        public string Email { get; set; }

        [Display(Name = "Contact No")]
        [Required(ErrorMessage = "Contact No Required!")]
        [RegularExpression(@"^[0-9]{1,20}$", ErrorMessage = "Only numbers are allowed.")]
        public string ContactNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address Required!")]
        public string Address { get; set; }
        public virtual Department Department { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "The Department field is required")]
        public int DepartmentId { get; set; }
    }
}