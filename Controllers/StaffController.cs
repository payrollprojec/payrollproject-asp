using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PayrollSystem.Models;

namespace PayrollSystem.Controllers
{
    [Authorize(Roles ="SuperAdmin, Admin")]
    public class StaffController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public StaffController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Staff
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName("Staff");

            var usersInRole = db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id)).ToList();
            return View(usersInRole);
            //return View(db.ApplicationUsers.ToList());
        }

        // GET: Staff/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var result = await manager.CreateAsync(applicationUser, "123456");
                if (result.Succeeded)
                {
                    manager.AddToRole(applicationUser.Id, "Staff");
                    return RedirectToAction("CreateSuccess", new { username = applicationUser.UserName, password = "123456" });

                }
            }

            return View(applicationUser);
        }

        public ActionResult CreateSuccess(string username, string password)
        {
            ViewBag.username = username;
            ViewBag.password = password;
            return View();
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser applicationUser)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(applicationUser).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(applicationUser);

            if (ModelState.IsValid)
            {
                ApplicationUser model = manager.FindById(applicationUser.Id);
                model.Email = applicationUser.Email;
                model.PhoneNumber = applicationUser.PhoneNumber;
                model.FullName = applicationUser.FullName;
                model.ICNo = applicationUser.ICNo;
                model.DateOfBirth = applicationUser.DateOfBirth;
                model.Address = applicationUser.Address;

                model.EPFNo = applicationUser.EPFNo;
                model.SocsoNo = applicationUser.SocsoNo;
                model.BasicSalary = applicationUser.BasicSalary;


                await manager.UpdateAsync(model);
                //db.Entry(user).State = EntityState.Modified;
                return RedirectToAction("Details/" + model.Id);
            }
            return View(applicationUser);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
