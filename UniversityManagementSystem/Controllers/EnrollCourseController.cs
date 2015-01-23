using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using UniversityManagementSystem.Models;

namespace UniversityManagementSystem.Controllers
{
    public class EnrollCourseController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /EnrollCourse/

        public ActionResult Index()
        {
            var enrollcourses = db.EnrollCourses.Include(e => e.Student).Include(e => e.Course);
            return View(enrollcourses.ToList());
        }

        //
        // GET: /EnrollCourse/Details/5

        public ActionResult Details(int id = 0)
        {
            EnrollCourse enrollcourse = db.EnrollCourses.Find(id);
            if (enrollcourse == null)
            {
                return HttpNotFound();
            }
            return View(enrollcourse);
        }

        //
        // GET: /EnrollCourse/Create

        //public ActionResult Create()
        //{
        //    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId");
        //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code");
        //    return View();
        //}

        public ViewResult Create(int? studentId)
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code");
            if (studentId != null)
            {
                //return View(db.Universities.Include(s => s.Student).Where(s => s.Student.DepartmentId == departmentId));
                return View();
            }
            else
            {
                return View();
            }
        }


        public PartialViewResult FilteredSection(int? studentId)
        {
            if (studentId != null)
            {
                ViewBag.Student = db.Students.Where(student => student.Id == studentId).ToList();
                var department =
                    db.Students.Where(student => student.Id == studentId)
                        .Select(dept => dept.DepartmentId).Select(dept => dept);
                int ret = 0;
                foreach (var dept in department)
                {
                    ret = dept;
                }
                ViewBag.Course = db.Courses.Where(x => x.DepartmentId == ret);
                ViewBag.Dept = db.Departments.Where(dept => dept.Id == ret).Select(dept => dept.Name);

                return PartialView("~/Views/Shared/_FilteredEnrollCourse.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredEnrollCourse.cshtml");
            }
        }

        //
        // POST: /EnrollCourse/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnrollCourse enrollcourse)
        {
            if (ModelState.IsValid)
            {
                var ret = db.EnrollCourses.Any(
                    s => (s.StudentId == enrollcourse.StudentId && s.CourseId == enrollcourse.CourseId));

                if (ret)
                {
                    TempData["error"] = "Course Already Enrolled to this student";
                    return RedirectToAction("Create");
                }
                else
                {
                    int sd = 0, cd = 0;
                    var studentDept = db.Students.Where(a => a.Id == enrollcourse.StudentId).Select(a => a.DepartmentId);
                    foreach (var i in studentDept)
                    {
                        sd = i;
                    }
                    var courseDept = db.Courses.Where(a => a.Id == enrollcourse.CourseId).Select(a => a.DepartmentId);
                    foreach (var i in courseDept)
                    {
                        cd = i;
                    }

                    if (sd == cd)
                    {
                        db.EnrollCourses.Add(enrollcourse);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "This course not belongs to student department(vice versa)";
                    }
                }
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId", enrollcourse.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", enrollcourse.CourseId);
            return View(enrollcourse);
        }

        //
        // GET: /EnrollCourse/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EnrollCourse enrollcourse = db.EnrollCourses.Find(id);
            if (enrollcourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId", enrollcourse.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", enrollcourse.CourseId);
            return View(enrollcourse);
        }

        //
        // POST: /EnrollCourse/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnrollCourse enrollcourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollcourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId", enrollcourse.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", enrollcourse.CourseId);
            return View(enrollcourse);
        }

        //
        // GET: /EnrollCourse/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EnrollCourse enrollcourse = db.EnrollCourses.Find(id);
            if (enrollcourse == null)
            {
                return HttpNotFound();
            }
            return View(enrollcourse);
        }

        //
        // POST: /EnrollCourse/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnrollCourse enrollcourse = db.EnrollCourses.Find(id);
            db.EnrollCourses.Remove(enrollcourse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}