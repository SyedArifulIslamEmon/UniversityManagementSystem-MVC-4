using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<UniversityDbContext>
    {
        protected override void Seed(UniversityDbContext context)
        {
            for (int i = 1; i < 9; i++)
            {
                context.Semesters.Add(new Semester { Name = "Semester " + i });    
            }
           
            context.Designations.Add(new Designation() {Name = "Examiner"});
            context.Designations.Add(new Designation() { Name = "Trainer" });
            context.Designations.Add(new Designation() { Name = "Lecturer" });
            context.Designations.Add(new Designation() { Name = "Professor" });

            string[] gradeLetter = {"A+", "A", "A-", "B+", "B", "B-", "C+", "C", "D", "F"};

            foreach (string g in gradeLetter)
            {
                context.Grades.Add(new Grade {GradeName = g});
            }

            string[] days = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};

            foreach (string d in days)
            {
                context.Days.Add(new Day {Name = d});
            }

            string[] rooms = { "A-101", "A-102", "A-103", "B-101", "B-102", "B-103", "C-101", "C-102", "C-103" };

            foreach (string r in rooms)
            {
                context.Rooms.Add(new Room { Name = r });
            }

            context.SaveChanges();
        }
    }
}