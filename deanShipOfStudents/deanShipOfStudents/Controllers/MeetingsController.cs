using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using deanShipOfStudents.Models;

namespace deanShipOfStudents.Controllers
{
    [Authorize]

    public class MeetingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Meetings
        public ActionResult Index(int? activityId)
        {
            if (activityId != null)
            {
                ViewBag.activityId = activityId;
                var meetings = db.Meetings.Include(m => m.Activity).Where(m=>m.ActivityId == activityId);
                return View(meetings.ToList());

            }
            else
            {
                var meetings = db.Meetings.Include(m => m.Activity);
                return View(meetings.ToList());

            }
        }

        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Include(m => m.Activity).Where(m => m.Id == id).ToList().FirstOrDefault();
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // GET: Meetings/Create
        public ActionResult Create(int? activityId)
        {
            if (activityId != null)
            {
                ViewBag.ActivityId = new SelectList(db.Activities.Where(a=>a.ActivityId == activityId), "ActivityId", "ActivityName",activityId);

            }
            else
            {
                ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "ActivityName");

            }
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateOfActivity,TimeOfActivity,ActivityId")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                db.Meetings.Add(meeting);
                db.SaveChanges();
                return RedirectToAction("Index",new { activityId = meeting.ActivityId });
            }

            ViewBag.ActivityId = new SelectList(db.Activities.Where(a => a.ActivityId == meeting.ActivityId), "ActivityId", "ActivityName", meeting.ActivityId);
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Include(m => m.Activity).Where(m => m.Id == id).ToList().FirstOrDefault();
            if (meeting == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities.Where(a => a.ActivityId == meeting.ActivityId), "ActivityId", "ActivityName", meeting.ActivityId);
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateOfActivity,TimeOfActivity,ActivityId")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { activityId = meeting.ActivityId });
            }
            ViewBag.ActivityId = new SelectList(db.Activities.Where(a => a.ActivityId == meeting.ActivityId), "ActivityId", "ActivityName", meeting.ActivityId);
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Include(m => m.Activity).Where(m => m.Id == id).ToList().FirstOrDefault();
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meeting meeting = db.Meetings.Include(m => m.Activity).Where(m => m.Id == id).ToList().FirstOrDefault();
            db.Meetings.Remove(meeting);
            db.SaveChanges();
            return RedirectToAction("Index", new { activityId = meeting.ActivityId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
