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
    public class TeacherController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.Designation).Include(t => t.Department);
            return View(teachers.ToList());
        }

        //
        // GET: /Teacher/Details/5

        public ActionResult Details(int id = 0)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        //
        // GET: /Teacher/Create

        public ActionResult Create()
        {
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code");
            return View();
        }

        //
        // POST: /Teacher/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Name", teacher.DesignationId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", teacher.DepartmentId);
            return View(teacher);
        }

        //
        // GET: /Teacher/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Name", teacher.DesignationId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", teacher.DepartmentId);
            return View(teacher);
        }

        //
        // POST: /Teacher/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Name", teacher.DesignationId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", teacher.DepartmentId);
            return View(teacher);
        }

        //
        // GET: /Teacher/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        //
        // POST: /Teacher/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult CheckEmail(string email)
        {
            var result = db.Teachers.Count(u => u.Email == email) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}