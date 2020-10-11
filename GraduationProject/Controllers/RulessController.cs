using GraduationProject.Models;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GraduationProject.Controllers
{
    [Authorize(Roles = "Admins")]
    public class RulessController : Controller
    {
        //
        // GET: /Ruless/
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        //
        // GET: /Ruless/Details/5
        public ActionResult Details(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // GET: /Ruless/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Ruless/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                    // TODO: Add insert logic here


                return View(role);
           
        }

        //
        // GET: /Ruless/Edit/5
        public ActionResult Edit(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Ruless/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include="Id,Name")] IdentityRole role)
        {

            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        //
        // GET: /Ruless/Delete/5
        public ActionResult Delete(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Ruless/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole role)
        {
            try
            {
                // TODO: Add delete logic here
                var MyRule = db.Roles.Find(role.Id);
                db.Roles.Remove(MyRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(role);
            }
        }
    }
}
