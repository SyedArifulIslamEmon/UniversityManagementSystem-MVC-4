using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityManagementSystem.Models;

namespace UniversityManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /Course/

        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Deparment).Include(c => c.Semester);
            return View(courses.ToList());
        }

        //
        // GET: /Course/Details/5

        public ActionResult Details(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // GET: /Course/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code");
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name");
            ViewBag.Teacher = db.Teachers.Select(teacher => teacher);
            ViewBag.Course = db.Courses.Select(course => course);
            return View();
        }

        //
        // POST: /Course/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", course.DepartmentId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", course.SemesterId);
            return View(course);
        }

        //
        // GET: /Course/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", course.DepartmentId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", course.SemesterId);
            return View(course);
        }

        //
        // POST: /Course/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", course.DepartmentId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", course.SemesterId);
            return View(course);
        }

        //
        // GET: /Course/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // POST: /Course/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult CheckCode(string code)
        {
            var result = db.Courses.Count(u => u.Code == code) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckName(string name)
        {
            var result = db.Courses.Count(u => u.Name == name) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}