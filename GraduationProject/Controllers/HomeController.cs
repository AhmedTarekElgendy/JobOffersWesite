using System;
using System.Net;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using GraduationProject.Models;
using Microsoft.AspNet.Identity;

namespace GraduationProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        [Authorize]
        public ActionResult Details(int JobId)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var job = db.Jobs.Find(JobId);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }
        [Authorize(Roles = "Applicants")]
        public ActionResult Apply()
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            return View(); 
        }
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var UserId = User.Identity.GetUserId();
            var JobId = (int)HttpContext.Session["JobId"];//(int)Session["JobId"];

            var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();
            if (check.Count() < 1)
            {
                var job = new ApplyForJob();
                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "Successful Apply";
            }
            else
            {
                ViewBag.Result = "Sorry, You Already Applied For This";
            }
            return View();
        }
        [Authorize(Roles = "Applicants")]
        public ActionResult GetJobUser()
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var UserId = User.Identity.GetUserId();
            var jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);

            return View(jobs.ToList());
        }
        [Authorize(Roles = "Applicants")]
        public ActionResult DetailsOfJob(int Id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var job = db.ApplyForJobs.Find(Id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        [Authorize(Roles = "Publishers")]
        public ActionResult GetJobByPublisher()
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var UserID = User.Identity.GetUserId();

            var Jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.Id
                       where job.user.Id == UserID
                       select app;

            var grouped = from j in Jobs
                          group j by j.job.JobTitle
                          into gr
                          select new JobsViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };

            return View(grouped.ToList());
        }
        [Authorize(Roles = "Applicants")]
        public ActionResult Edit(int id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobUser");
            }
            return View(job);
        }
        [Authorize(Roles = "Applicants")]
        public ActionResult Delete(int id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index");
            }
            // TODO: Add delete logic here
            var MyJob = db.ApplyForJobs.Find(job.Id);
                db.ApplyForJobs.Remove(MyJob);
                db.SaveChanges();
                return RedirectToAction("GetJobUser");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
         [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("mail", "Pass");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("mail"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;

            string body = "Name: " + contact.Name + "<br>" +
                          "Email: " + contact.Email + "<br>" +
                          "Title: " + contact.Subject + "<br>" +
                          "Message: " + contact.Message + "<br>";
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);

            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(searchName)
            || a.JobContent.Contains(searchName)
            || a.Category.CategoryName.Contains(searchName)
            || a.Category.CategoryDescription.Contains(searchName));
            return View(result);
        }
    }
}