using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using UniversityManagementSystem.Models;

namespace UniversityManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /Student/

        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        //
        // GET: /Student/Details/5

        public ActionResult Details(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code");
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                student.RegistrationId = GenerateStudentRegistrationId(student);
                ViewBag.registrationId = student.RegistrationId;
                db.Students.Add(student);
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", student.DepartmentId);
                return View();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", student.DepartmentId);
            return View(student);
        }

        private string GenerateStudentRegistrationId(Student student)
        {
            string registrationId = GetDepartmentCode(student.DepartmentId) + "-" + student.Date.Year + "-";
            var result = db.Students.Where(s => (s.DepartmentId == student.DepartmentId && s.Date.Year == student.Date.Year)).ToList();

            if (!result.Any())
            {
                registrationId += "001";
            }
            else
            {
                string studentRegistrationId = (result.Last().RegistrationId);
                studentRegistrationId = (Convert.ToInt32(studentRegistrationId.Substring(studentRegistrationId.Length - 3)) + 1).ToString();
                
                int len = 3 - studentRegistrationId.Length;
                for (var i = 0; i <len; i++)
                {
                    studentRegistrationId = "0" + studentRegistrationId;
                }
                registrationId += studentRegistrationId;
            }
            return registrationId;
        }

        private string GetDepartmentCode(int departmentId)
        {
            var result =
                db.Departments.Where(departments => departments.Id == departmentId)
                    .Select(departments => new {departments.Code});

            return Enumerable.FirstOrDefault(result.Select(code => code.Code));
        }


        //
        // GET: /Student/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", student.DepartmentId);
            return View(student);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", student.DepartmentId);
            return View(student);
        }

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        public JsonResult CheckEmail(string email)
        {
            var result = db.Students.Count(u => u.Email == email) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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