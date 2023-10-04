using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data;

namespace deanShipOfStudents.Controllers
{
    [Authorize]

    public class CertificateController : Controller
    {
        // GET: Certificate
        private deanShipOfStudents.Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        public ActionResult NotifiTaineeAboutCertificate(int activityId)
        {

            var users = from a in db.Activities.Where(a => a.ActivityId == activityId)
                        join t in db.TraineeActivities on a.ActivityId equals t.ActivityId
                        join b in db.Users.Where(u => u.userType == Models.ApplicationUser.UserType.trainee) on t.TraineeId equals b.Id
                        select new
                        {
                            email = b.Email,
                            activityName = a.ActivityName,
                            traineeName = b.firstName
                        };
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            mail.From = new MailAddress("seniorprojectkau1999@gmail.com");
            try
            {

                foreach ( var item in users.ToList() )
                {

                    mail.To.Add(item.email);
                    mail.Subject = item.activityName+" certificate";
                    mail.Body = "Dear "+item.traineeName + " your certificate for " + item.activityName + "is ready to download ";
                    mail.IsBodyHtml = false;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("seniorprojectkau1999@gmail.com", "Senior@Project99");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                ViewBag.error = "trainees have been notified for there certificate";
            }
            catch (Exception e)
            {

                ViewBag.error = e.Message;
            }

            return View();
        }

        public ActionResult getTraineeCertificate(int activityId)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name && u.userType == Models.ApplicationUser.UserType.trainee).ToList().FirstOrDefault();
            var attendance = db.Attatchments.Where(a => a.activityId == activityId && a.traineId == user.Id && a.type == Models.Attatchment.FileType.attendance).ToList();
            var avgAttendance = 0.0;
            if (attendance != null && attendance.Count() > 0)
            {
                avgAttendance = attendance.Select(a => a.degree).Average();
            }
            var evaluation = db.Attatchments.Where(a => a.activityId == activityId && a.traineId == user.Id && a.type == Models.Attatchment.FileType.evaluation).ToList();
            var avgEvaluation = 0.0;
            if (evaluation != null && evaluation.Count() > 0)
            {
                avgEvaluation = evaluation.Select(a => a.degree).Average();
            }
            var traineeActivity = from a in db.Activities.Where(a => a.ActivityId == activityId)
                                  join ta in db.TraineeActivities on a.ActivityId equals ta.ActivityId
                                  join t in db.Users.Where(u => u.Id == user.Id) on ta.TraineeId equals t.Id

                                  select new
                                  {
                                      ActivityName = a.ActivityName,
                                      Location = a.Location,
                                      attendance = avgAttendance,
                                      evaluation = avgEvaluation,
                                      traineeName = user.firstName + " " + user.lastName,
                                      traineeId = user.UniverstyId
                                  };
            var act = traineeActivity.ToArray();
            traineActivityModel traineActivity = new traineActivityModel();
            traineActivity.ActivityName = act[0].ActivityName;
            traineActivity.Location = act[0].Location;
            traineActivity.AvgAttendance = act[0].attendance;
            traineActivity.AvgEvaluation = act[0].evaluation;
            traineActivity.TraineeName = act[0].traineeName;
            traineActivity.UniverstyId = act[0].traineeId;
            ViewBag.traineeActivity = traineActivity;

            ////////////////////////
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/CrystalReport1.rpt")));
            DataTable docDt = new DataTable("CertificateView");
            docDt.Columns.Add("ActivityName");
            docDt.Columns.Add("Location");
            docDt.Columns.Add("AvgAttendance");
            docDt.Columns.Add("AvgEvaluation");
            docDt.Columns.Add("TraineeName");
            docDt.Columns.Add("UniverstyId");
          
            var student = db.Users.Where(u=>u.UniverstyId == traineActivity.UniverstyId).ToList().FirstOrDefault();
           
            DataRow dataRow = docDt.NewRow();
            dataRow[0] = traineActivity.ActivityName;
            dataRow[1] = traineActivity.Location;
            dataRow[2] = traineActivity.AvgAttendance;
            dataRow[3] = traineActivity.AvgEvaluation;
            dataRow[4] = traineActivity.TraineeName;
            dataRow[5] = traineActivity.UniverstyId;
            docDt.Rows.Add(dataRow);
            DataSet ds = new DataSet("Certificate");
            ds.Tables.Add(docDt);
            rd.SetDataSource(ds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string SavedFileName = string.Format("Document{0}", traineActivity.UniverstyId + ".pdf");
            string path = Server.MapPath(string.Format("~/Doc/{0}", SavedFileName));
            //  var file = File(stream, "application/pdf", SavedFileName);
            //using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            //{
            //    stream.CopyTo(fileStream);
            //}re
           
           

            return File(stream, "application/pdf", SavedFileName);
        }
    }

   
}