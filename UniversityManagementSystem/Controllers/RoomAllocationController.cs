using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using UniversityManagementSystem.Models;

namespace UniversityManagementSystem.Controllers
{
    public class RoomAllocationController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        //
        // GET: /RoomAllocation/

        public ActionResult Index()
        {
            var roomallocations = db.RoomAllocations.Include(r => r.Department).Include(r => r.Course).Include(r => r.Room).Include(r => r.Day);
            return View(roomallocations.ToList());
        }

        //
        // GET: /RoomAllocation/Details/5

        public ActionResult Details(int id = 0)
        {
            RoomAllocation roomallocation = db.RoomAllocations.Find(id);
            if (roomallocation == null)
            {
                return HttpNotFound();
            }
            return View(roomallocation);
        }

        //
        // GET: /RoomAllocation/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name");
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name");
            return View();
        }

        //
        // POST: /RoomAllocation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomAllocation roomallocation)
        {
            
            if (ModelState.IsValid)
            {
                if (roomallocation.StartTime >= roomallocation.EndTime)
                {
                    GetSelectValue(roomallocation);
                    @ViewData["error"] = "Start Time should be less than End Time";
                    return View(roomallocation);
                }

                var res =
                    db.RoomAllocations.Where(
                        (a =>
                            (a.RoomId == roomallocation.RoomId && a.DayId == roomallocation.DayId &&
                             (a.StartTime < roomallocation.EndTime && roomallocation.StartTime < a.EndTime))))
                        .Include(a => a.Department)
                        .Include(a => a.Course);

                if (res.Any())
                {
                    @ViewBag.roomAllocationTable = res;
                    View();
                }
                else
                {
                    db.RoomAllocations.Add(roomallocation);
                    db.SaveChanges();
                    return RedirectToAction("ViewClassSchedule");
                }
            }

            GetSelectValue(roomallocation);
            return View(roomallocation);
        }

        private void GetSelectValue(RoomAllocation roomallocation)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", roomallocation.DepartmentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", roomallocation.CourseId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
        }

        //
        // GET: /RoomAllocation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoomAllocation roomallocation = db.RoomAllocations.Find(id);
            if (roomallocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", roomallocation.DepartmentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", roomallocation.CourseId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
            return View(roomallocation);
        }

        //
        // POST: /RoomAllocation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomAllocation roomallocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomallocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code", roomallocation.DepartmentId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Code", roomallocation.CourseId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
            return View(roomallocation);
        }

        //
        // GET: /RoomAllocation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RoomAllocation roomallocation = db.RoomAllocations.Find(id);
            if (roomallocation == null)
            {
                return HttpNotFound();
            }
            return View(roomallocation);
        }

        //
        // POST: /RoomAllocation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomAllocation roomallocation = db.RoomAllocations.Find(id);
            db.RoomAllocations.Remove(roomallocation);
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
                ViewBag.Courses = db.Courses.Where(course => course.DepartmentId == departmentId);
                return PartialView("~/Views/Shared/_FilteredCoursesRoomAllocation.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_FilteredCoursesRoomAllocation.cshtml");
            }
        }

        public PartialViewResult FilteredClassScheduleAndRoomAllocationInfo(int? departmentId)
        {
            if (departmentId == null)
            {
                return PartialView("~/Views/Shared/_ViewClassSchedule.cshtml");
            }
            //var ret =
            //    db.RoomAllocations.Include(a => a.Department)
            //        .Include(a => a.Course)
            //        .Include(a => a.Room)
            //        .Include(a => a.Day)
            //        .Where(a => a.DepartmentId == departmentId)
            //        .GroupBy(a => a.Course.Code)
            //        .Select(g => new { CourseCode = g.Key, ScheduleInfo = g });

            var ret =
                db.RoomAllocations.Include(a => a.Department)
                    .Include(a => a.Course)
                    .Include(a => a.Room)
                    .Include(a => a.Day)
                    .Where(a => a.DepartmentId == departmentId)
                    .GroupBy(a => a.Course.Code)
                    .Select(g => new { CourseCode = g.Key, ScheduleInfo = g });

            var result = new List<ViewClassSchedule>();
            
            var aDictionary = new Dictionary<string, int>();

            foreach (var g in ret)
            {
                var roomAllocations = new List<RoomAllocation>();
                var courseName = "";
                foreach (var n in g.ScheduleInfo)
                {
                    roomAllocations.Add(new RoomAllocation
                    {
                        Day = n.Day,
                        Room = n.Room,
                        StartTime = n.StartTime,
                        EndTime = n.EndTime,
                    });
                    courseName = n.Course.Name;
                }
                result.Add(new ViewClassSchedule { CourseCode = g.CourseCode, CourseName = courseName, RoomAllocations = roomAllocations });
                aDictionary.Add(g.CourseCode, 1);
            }

            var courses = db.Courses.Where(a => a.DepartmentId == departmentId);
            foreach (var course in courses)
            {
                if (!aDictionary.ContainsKey(course.Code))
                {
                    result.Add(new ViewClassSchedule { CourseCode = course.Code, CourseName = course.Name, RoomAllocations = null });
                }
            }
            if (result.Count < 1)
            {
                TempData["error"] = "There is no course in this department to allocate";
            }
            return PartialView("~/Views/Shared/_ViewClassSchedule.cshtml", result);
        }

        public ActionResult ViewClassSchedule()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Code");
            return View();
        }
    }
}