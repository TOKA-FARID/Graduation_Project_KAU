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

    public class SuggestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Suggestions
        public ActionResult Index(int? ActivityId)
        {
            if (ActivityId != null)
            {
                var suggestions = db.Suggestions.Include(s => s.activity).Where(s=>s.activityId == ActivityId);
                ViewBag.ActivityId = ActivityId;
                return View(suggestions.ToList());

            }
            else
            {
                var suggestions = db.Suggestions.Include(s => s.activity);
                return View(suggestions.ToList());

            }
        }

        // GET: Suggestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = db.Suggestions.Include(s => s.activity).Where(s => s.activityId == id).ToList().FirstOrDefault();
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            return View(suggestion);
        }

        // GET: Suggestions/Create
        public ActionResult Create(int? ActivityId)
        {
            if (ActivityId != null)
            {
                ViewBag.activityId = new SelectList(db.Activities.Where(a=>a.ActivityId == ActivityId), "ActivityId", "ActivityName",ActivityId);

            }
            else
            {
                ViewBag.activityId = new SelectList(db.Activities, "ActivityId", "ActivityName");

            }
            return View();
        }

        // POST: Suggestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,text,activityId")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                db.Suggestions.Add(suggestion);
                db.SaveChanges();
                return RedirectToAction("Index",new { ActivityId = suggestion.activityId });
            }

            ViewBag.activityId = new SelectList(db.Activities, "ActivityId", "ActivityName", suggestion.activityId);
            return View(suggestion);
        }

        // GET: Suggestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = db.Suggestions.Include(s => s.activity).Where(s => s.activityId == id).ToList().FirstOrDefault();
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.activityId = new SelectList(db.Activities, "ActivityId", "ActivityName", suggestion.activityId);
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,text,activityId")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suggestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ActivityId = suggestion.activityId });
            }
            ViewBag.activityId = new SelectList(db.Activities, "ActivityId", "ActivityName", suggestion.activityId);
            return View(suggestion);
        }

        // GET: Suggestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = db.Suggestions.Include(s => s.activity).Where(s => s.activityId == id).ToList().FirstOrDefault();
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            return View(suggestion);
        }

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suggestion suggestion = db.Suggestions.Include(s => s.activity).Where(s => s.activityId == id).ToList().FirstOrDefault();
            db.Suggestions.Remove(suggestion);
            db.SaveChanges();
            return RedirectToAction("Index", new { ActivityId = suggestion.activityId });
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
