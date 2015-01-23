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
    public class ResultController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /Result/

        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.Student).Include(r => r.Course).Include(r => r.Grade);
            return View(results.ToList());
        }

        //
        // GET: /Result/Details/5

        public ActionResult Details(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //
        // GET: /Result/Create

        //public ActionResult Create()
        //{
        //    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId");
        //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code");
        //    ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
        //    return View();
        //}

        public ViewResult Create(int? studentId)
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code");
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
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

        public PartialViewResult FilteredSection2(int? studentId)
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
                //ViewBag.Course = db.Courses.Where(x => x.DepartmentId == ret);
                //ViewBag.Result = db.Results.Include(r => r.Student).Include(r => r.Course).Include(r => r.Grade).Where(student => student.StudentId == studentId);
                var x =
                    db.Results.Include(r => r.Student)
                        .Include(r => r.Course)
                        .Include(r => r.Grade)
                        .Where(student => student.StudentId == studentId);
                ViewBag.Result =
                    from ac in db.EnrollCourses.Where(student => student.StudentId == studentId)
                    join xx in x
                        on ac.CourseId equals xx.CourseId
                        into yG
                    from y1 in yG.DefaultIfEmpty()
                    select
                        new ResultView
                        {
                            CourseCode = ac.Course.Code,
                            CourseName = ac.Course.Name,
                            CourseGrade = y1.Grade.GradeName ?? "Not Yet Graded"
                        };
                

                ViewBag.Dept = db.Departments.Where(dept => dept.Id == ret).Select(dept => dept.Name);
                return PartialView("~/Views/Shared/_FilteredViewResult2.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredViewResult2.cshtml");
            }
        }

        //
        // POST: /Result/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Result result)
        {
            if (ModelState.IsValid)
            {
                var courseEnrollCheck =
                    db.EnrollCourses.Any(
                        student => student.StudentId == result.StudentId && student.CourseId == result.CourseId);
                if (courseEnrollCheck)
                {
                    var ret = db.Results.Any(res => res.StudentId == result.StudentId && res.CourseId == result.CourseId);

                    if (!ret)
                    {
                        db.Results.Add(result);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //TempData["error"] = "The subject " + result.Course.Name + " of the student " + result.Student.RegistrationId +" is already Graded";
                        TempData["error"] = "The subject of this student is already graded";
                        return RedirectToAction("Create");
                    }
                }
                else
                {
                    TempData["error"] = "The subject you selected is not enrolled by this student";
                    return RedirectToAction("Create");
                }
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId", result.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", result.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", result.GradeId);
            return View(result);
        }

        //
        // GET: /Result/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId", result.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", result.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", result.GradeId);
            return View(result);
        }

        //
        // POST: /Result/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationId", result.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", result.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", result.GradeId);
            return View(result);
        }

        //
        // GET: /Result/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //
        // POST: /Result/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ViewResult()
        {
            ViewBag.results = db.Students.Select(student => student).ToList();
            return View();
        }
    }
}