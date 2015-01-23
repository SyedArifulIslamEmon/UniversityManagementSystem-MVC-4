using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace UniversityManagementSystem.Models
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<Department> Departments { set; get; }
        public DbSet<Semester> Semesters { set; get; }
        public DbSet<Course> Courses { set; get; }
        public DbSet<Designation> Designations { set; get; }
        public DbSet<Teacher> Teachers { set; get; }
        public DbSet<Student> Students { set; get; }
        public DbSet<AssignCourse> AssignCourses { set; get; }

        public DbSet<EnrollCourse> EnrollCourses { set; get; }
        public DbSet<Grade> Grades { set; get; }
        public DbSet<Result> Results { set; get; }

        public DbSet<Room> Rooms { set; get; }

        public DbSet<Day> Days { set; get; } 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<RoomAllocation> RoomAllocations { get; set; }

    }
}