using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class AssignCourseView
    {
        public string CourseCode { set; get; }
        public string CourseName { set; get; }
        public string SemesterName { set; get; }
        public string TeacherName { set; get; }
    }
}