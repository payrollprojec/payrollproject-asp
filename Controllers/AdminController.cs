using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayrollSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public AdminController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // localhost:/Admin/
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName("Admin");

            var usersInRole = db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id)).ToList();
            return View(usersInRole);
        }

        // localhost:/Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AdminViewModel admin)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = admin.UserName,
                    FullName = admin.FullName,
                    Email = admin.Email,
                    PhoneNumber = admin.PhoneNumber,
                    ICNo = admin.ICNo,
                };
                var result = await manager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                    return RedirectToAction("CreateSuccess", new { username = admin.UserName, password = "123456" });
                }
                AddErrors(result);
            }

            return View(admin);
        }

        public ActionResult CreateSuccess(string username, string password)
        {
            ViewBag.username = username;
            ViewBag.password = password;
            return View();
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = manager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = manager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser model = manager.FindById(user.Id);
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
                model.FullName = user.FullName;
                model.ICNo = user.ICNo;
                await manager.UpdateAsync(model);
                //db.Entry(user).State = EntityState.Modified;
                return RedirectToAction("Index");
            }
            return View(user);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}