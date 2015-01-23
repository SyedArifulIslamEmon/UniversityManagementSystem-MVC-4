using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class ViewClassSchedule
    {
        public string CourseCode { set; get; }
        public string CourseName { set; get; }
        public List<RoomAllocation> RoomAllocations { set; get; } 
    }
}