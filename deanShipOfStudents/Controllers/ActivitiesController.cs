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
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();

            if (User.IsInRole("Admin"))
            {
                return View(db.Activities.ToList());
            }
            else if (User.IsInRole("Supervisor"))
            {
                var matches = from c in db.Centers
                              where c.SupervisorId == user.Id
                              join ca in db.CenterActivities
                              on c.CenterId equals ca.CenterId
                              join a in db.Activities on ca.ActivityId equals a.ActivityId
                           
                              select new
                              {
                                  a.ActivityId,
                                  a.ActivityName,
                                  a.Location,
                                  a.IsAssigned,
                                  a.IsClosed,
                                  a.type
                              };
                List<Activity> activities = new List<Activity>();
                foreach (var item in matches)
                {
                    activities.Add(new Activity {
                        ActivityId = item.ActivityId,
                        ActivityName = item.ActivityName,
                        IsAssigned = item.IsAssigned,
                        IsClosed = item.IsClosed,
                        Location = item.Location,
                        type = item.type
                        });
                }
                return View(activities);

            }
            else
            {
                return View();
            }
           
        }

        public ActionResult GetTrainerActivites(string userId)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();
            var activities = from a in db.Activities.Include(m=>m.Meetings).Include(m=>m.Matrials)
                             join b in db.TrainerActivities
                             on a.ActivityId equals b.ActivityId
                             join u in db.Users.Where(s=>s.Id == user.Id)
                             on b.TrainerId equals u.Id
                             select new
                             {
                                 a.ActivityId,
                                 a.ActivityName,
                                 a.IsAssigned,
                                 a.IsClosed,
                                 a.Location,
                                 a.type,
                                 a.Meetings,
                                 a.Matrials
                             };

            List<Activity> activities1 = new List<Activity>();
            foreach (var item in activities.ToList())
            {
                activities1.Add(new Activity
                {
                    ActivityId = item.ActivityId,
                    ActivityName = item.ActivityName,
                    IsAssigned = item.IsAssigned,
                    IsClosed = item.IsClosed,
                    Location = item.Location,
                    Meetings = item.Meetings,
                    Matrials = item.Matrials,
                    type = item.type,

                });
            }
            return View(activities1);
        }
        public ActionResult getTraineeAvilableActivities(string userId)
        {
            var allActivites = db.Activities.Where(a => a.IsClosed == false).ToList();
            var userActivities = from a in db.Activities.Include(a => a.Meetings).Where(a => a.IsClosed == false)
                                join b in db.TraineeActivities.Where(ta => ta.TraineeId == userId) on a.ActivityId equals b.ActivityId

                                select new
                                {
                                    ActivityId = a.ActivityId,
                                    ActivityName = a.ActivityName,
                                    IsClosed = a.IsClosed,
                                    Location = a.Location,
                                    type = a.type,
                                    Meetings = a.Meetings,
                                    Matrials = a.Matrials,
                                    IsAssigned = a.IsAssigned
                                };
            bool isFound = false;
            List<Activity> activities1 = new List<Activity>();
            foreach (var activity in allActivites)
            {
                isFound = false;
                foreach (var userActivity in userActivities.ToList())
                {
                    if (activity.ActivityId == userActivity.ActivityId)
                    {
                        isFound = true;
                    }
                }
                if (isFound == false)
                {
                    activities1.Add(activity);
                }
            }
       
            return View(activities1);
        }
        public ActionResult getTraineeActivities(string userId)
        {

            var activites = from a in db.Activities.Include(a => a.Meetings).Include(a=>a.Matrials)
                            join b in db.TraineeActivities.Where(ta => ta.TraineeId == userId) on a.ActivityId equals b.ActivityId 
                         
                            select new
                            {
                                ActivityId = a.ActivityId,
                                ActivityName = a.ActivityName,
                                IsClosed = a.IsClosed,
                                Location = a.Location,
                                type = a.type,
                                Meetings = a.Meetings,
                                Matrials = a.Matrials,
                                IsAssigned = a.IsAssigned
                            };
            List<Activity> activities1 = new List<Activity>();
            foreach (var item in activites.ToList())
            {
                activities1.Add(new Activity
                {
                    ActivityId = item.ActivityId,
                    ActivityName = item.ActivityName,
                    IsAssigned = item.IsAssigned,
                    IsClosed = item.IsClosed,
                    Location = item.Location,
                    Meetings = item.Meetings,
                    type = item.type,
                    Matrials = item.Matrials

                });
            }
            return View(activities1);
        }
        [HttpPost]
        public ActionResult enrollTraineeActiviy( int ActivityId)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();

            try
            {
                if (user != null)
                {
                    TraineeActivites traineeActivites = new TraineeActivites();
                    traineeActivites.ActivityId = ActivityId;
                    traineeActivites.TraineeId = user.Id;
                    db.TraineeActivities.Add(traineeActivites);
                    db.SaveChanges();
                    return RedirectToAction("getTraineeActivities", new { userId = user.Id });
                }
                else
                {
                    return RedirectToAction("login","account");
                }
               

            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return RedirectToAction("getTraineeAvilableActivities", new { userId = user.Id });
            }

        }
        public ActionResult cancelTraineeActiviy(int ActivityId)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();

            try
            {
                if (user != null)
                {
                    TraineeActivites traineeActivites = db.TraineeActivities.Where(t => t.ActivityId == ActivityId && t.TraineeId == user.Id).ToList().FirstOrDefault();
                    if (traineeActivites != null)
                    {
                        db.TraineeActivities.Remove(traineeActivites);
                        db.SaveChanges();
                        return RedirectToAction("getTraineeActivities", new { userId = user.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", "error in remove activity from trainee account");
                        return RedirectToAction("getTraineeActivities", new { userId = user.Id });
                    }
                   
                }
                else
                {
                    return RedirectToAction("login", "account");
                }


            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return RedirectToAction("getTraineeAvilableActivities", new { userId = user.Id });
            }

        }
        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Include(a=>a.Meetings).Where(a=>a.ActivityId == id).ToList().FirstOrDefault();
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();
            ViewBag.TrainerId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.trainer && u.isActive), "Id", "firstName");
            if (User.IsInRole("Admin"))
            {
                ViewBag.centerId = new SelectList(db.Centers, "CenterId", "CenterName");

            }
            else if (User.IsInRole("Supervisor"))
            {
               
                ViewBag.centerId = new SelectList(db.Centers.Where(c=>c.SupervisorId == user.Id), "CenterId", "CenterName");

            }

            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Activity activity , string TrainerId,int centerId)
        {
            if (ModelState.IsValid)
            {
                
                db.Activities.Add(activity);
                if (activity.IsAssigned)
                {
                    TrainerActivities trainerActivities = new TrainerActivities();
                    trainerActivities.ActivityId = activity.ActivityId;
                    trainerActivities.TrainerId = TrainerId;
                    db.TrainerActivities.Add(trainerActivities);
                   
                }
                if (centerId != 0)
                {
                    CenterActivities centerActivities = new CenterActivities();
                    centerActivities.CenterId = centerId;
                    centerActivities.ActivityId = activity.ActivityId;
                    db.CenterActivities.Add(centerActivities);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();
            ViewBag.TrainerId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.trainer && u.isActive), "Id", "firstName");
            if (User.IsInRole("Admin"))
            {
                ViewBag.centerId = new SelectList(db.Centers, "CenterId", "CenterName");

            }
            else if (User.IsInRole("Supervisor"))
            {
                var matches = from a in db.Centers
                              join r in db.CenterActivities
                              on a.CenterId equals r.CenterId
                              where a.SupervisorId == user.Id
                              select new
                              {
                                  a.CenterId,
                                  a.CenterName
                              };
                ViewBag.centerId = new SelectList(matches, "CenterId", "CenterName");

            }


            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).ToList().FirstOrDefault();
            if (activity.TrainerActivities != null)
            {
                ViewBag.TrainerId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.trainer && u.isActive), "Id", "firstName",activity.TrainerActivities.FirstOrDefault().TrainerId);

            }
            else
            {
                ViewBag.TrainerId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.trainer && u.isActive), "Id", "firstName");

            }
            if (User.IsInRole("Admin"))
            {
                if (activity.CentersActivities != null)
                {
                    ViewBag.centerId = new SelectList(db.Centers, "CenterId", "CenterName" ,activity.CentersActivities.FirstOrDefault().CenterId);

                }
                else
                {
                    ViewBag.centerId = new SelectList(db.Centers, "CenterId", "CenterName");

                }

            }
            else if (User.IsInRole("Supervisor"))
            {
                var matches = from a in db.Centers
                              join r in db.CenterActivities
                              on a.CenterId equals r.CenterId
                              where a.SupervisorId == user.Id
                              select new
                              {
                                  a.CenterId,
                                  a.CenterName
                              };
                ViewBag.centerId = new SelectList(matches, "CenterId", "CenterName",matches.FirstOrDefault().CenterId);

            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
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
