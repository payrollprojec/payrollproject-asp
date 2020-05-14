namespace PayrollSystem.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PayrollSystem.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PayrollSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PayrollSystem.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string name = "superadmin";
            string[] roles = new string[] { "SuperAdmin", "Admin", "Staff" };

            // if superadmin is not yet created, create 3 roles and a superadmin
            if (UserManager.FindByName(name) != null)
            {
                foreach (string role in roles)
                {
                    RoleManager.Create(new IdentityRole(role));
                }

                var user = new ApplicationUser
                {
                    UserName = name
                };
                var adminresult = UserManager.Create(user, name);

                if (adminresult.Succeeded)
                {
                    UserManager.AddToRole(user.Id, roles[0]);
                }

                var user2 = new ApplicationUser
                {
                    UserName = "staff1",
                    FullName = "Jonathan",
                    ICNo = "661010105555"
                };
                UserManager.Create(user2, "staff1");
                UserManager.AddToRole(user2.Id, roles[2]);
                var user3 = new ApplicationUser
                {
                    UserName = "admin1",
                    FullName = "Alfred",
                    ICNo = "681010105555"
                };
                UserManager.Create(user3, "admin1");
                UserManager.AddToRole(user3.Id, roles[1]);
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
