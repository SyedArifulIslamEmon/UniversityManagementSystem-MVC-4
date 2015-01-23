using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UniversityManagementSystem.Models;


namespace UniversityManagementSystem.Controllers
{
    public class AssignCourseController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /AssignCourse/

        public ActionResult Index()
        {
            var assigncourses = db.AssignCourses.Include(a => a.Department).Include(a => a.Teacher).Include(a => a.Course);
            return View(assigncourses.ToList());
        }

        //
        // GET: /AssignCourse/Details/5

        public ActionResult Details(int id = 0)
        {
            AssignCourse assigncourse = db.AssignCourses.Find(id);
            if (assigncourse == null)
            {
                return HttpNotFound();
            }
            return View(assigncourse);
        }

        //
        // GET: /AssignCourse/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code");
            return View();
        }

        //
        // POST: /AssignCourse/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssignCourse assigncourse)
        {
            if (ModelState.IsValid)
            {
                var alreadyAssigned = db.AssignCourses.Any(a => a.CourseId == assigncourse.CourseId);
                if (alreadyAssigned)
                {
                    var teacherName =
                        db.AssignCourses.Include(a => a.Teacher)
                            .Where(a => a.CourseId == assigncourse.CourseId)
                            .Select(a => a.Teacher.Name);
                    
                    var teacher = "";
                    
                    foreach (var t in teacherName)
                    {
                        teacher = t;
                    }

                    TempData["error"] = "The Course is already Assigned to " + teacher;
                }
                else
                {
                    int td = 0, cd = 0;
                    var teacherDept = db.Teachers.Where(a => a.Id == assigncourse.TeacherId).Select(a => a.DepartmentId);
                    foreach (var i in teacherDept)
                    {
                        td = i;
                    }
                    var courseDept = db.Courses.Where(a => a.Id == assigncourse.CourseId).Select(a => a.DepartmentId);
                    foreach (var i in courseDept)
                    {
                        cd = i;
                    }

                    if (td == cd)
                    {
                        db.AssignCourses.Add(assigncourse);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "This course not belongs to teachers department(vice versa)";
                    }
                }
                
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", assigncourse.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", assigncourse.TeacherId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", assigncourse.CourseId);
            return View(assigncourse);
        }

        //
        // GET: /AssignCourse/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AssignCourse assigncourse = db.AssignCourses.Find(id);
            if (assigncourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", assigncourse.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", assigncourse.TeacherId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", assigncourse.CourseId);
            return View(assigncourse);
        }

        //
        // POST: /AssignCourse/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AssignCourse assigncourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assigncourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", assigncourse.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", assigncourse.TeacherId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", assigncourse.CourseId);
            return View(assigncourse);
        }

        //
        // GET: /AssignCourse/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AssignCourse assigncourse = db.AssignCourses.Find(id);
            if (assigncourse == null)
            {
                return HttpNotFound();
            }
            return View(assigncourse);
        }

        //
        // POST: /AssignCourse/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignCourse assigncourse = db.AssignCourses.Find(id);
            db.AssignCourses.Remove(assigncourse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public PartialViewResult FilteredSection(int? departmentId)
        {
            if (departmentId != null)
            {
                ViewBag.Teachers = db.Teachers.Where(teacher => teacher.DepartmentId == departmentId);
                return PartialView("~/Views/Shared/_FilteredAssignCourseTeacher.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredAssignCourseTeacher.cshtml");
            }
        }

        public PartialViewResult FilteredSection2(int? departmentId)
        {
            if (departmentId != null)
            {
                ViewBag.Courses = db.Courses.Where(course => course.DepartmentId == departmentId);
                return PartialView("~/Views/Shared/_FilteredAssignCourseCombo.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredAssignCourseCombo.cshtml");
            }
        }

        public PartialViewResult FilteredSectionCourse(int? courseId)
        {
            if (courseId != null)
            {
                ViewBag.Course = db.Courses.Where(course => course.Id == courseId);
                return PartialView("~/Views/Shared/_FilteredCourse.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredCourse.cshtml");
            }
        }

        public PartialViewResult FilteredSectionCredit(int? teacherId)
        {
            if (teacherId != null)
            {
                 ViewBag.CreditToBeTaken =
                    from a in db.AssignCourses
                    join c in db.Courses
                        on a.CourseId equals c.Id
                    where a.TeacherId == teacherId
                    group c by a.TeacherId
                    into g
                    select new TotalCredit{Credit = g.Sum(c => c.Credit)};

                ViewBag.AssigningCredit =
                    db.Teachers.Where(teacher => teacher.Id == teacherId).Select(teacher => teacher.Credit);

                return PartialView("~/Views/Shared/_FilteredAssignCourse.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredAssignCourse.cshtml");
            }
        }

        public ActionResult CourseStatus()
        {
            ViewBag.Dept = db.Departments;
            return View();
        }


        public PartialViewResult FilteredCourseStatus(int? departmentId)
        {
            if (departmentId != null)
            {

                var ret = from c in db.Courses.Where(dept => dept.DepartmentId == departmentId)
                    join t in db.AssignCourses on c.Id equals t.CourseId into yG
                    from y1 in yG.DefaultIfEmpty()
                    select
                        new AssignCourseView
                        {
                            CourseCode = c.Code,
                            CourseName = c.Name,
                            SemesterName = c.Semester.Name,
                            TeacherName = y1.Teacher.Name
                        };

                if (!ret.Any())
                {
                    TempData["error"] = "There is no course in This department";
                }
                ViewBag.Result = ret;
                //var ab = departments.Include(d => d.Customers.Select(c => c.Orders));
                return PartialView("~/Views/Shared/_FilteredCourseStatus.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredCourseStatus.cshtml");
            }

        }
    }
}