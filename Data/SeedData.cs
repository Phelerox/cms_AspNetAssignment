using cms.Authorization;
using cms.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
// dotnet aspnet-codegenerator razorpage -m Person -dc ApplicationDbContext -outDir Pages\Persons --referenceScriptLibraries
namespace cms.Data
{
    public static class SeedData
    {
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "baxemyr@gmail.com");
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorsRole);

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        #endregion
        #region snippet1
        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Persons.Any())
            {
                return;   // DB has been seeded
            }

            context.Persons.AddRange(
            #region snippet_Person
                new Person
                {
                    Name = "Debra Garcia",
                    // City = ,
                    Email = "debra@example.com",
                    Status = PersonStatus.Visible,
                    CreatorId = adminID
                },
            #endregion
            #endregion
                new Person
                {
                    Name = "Thorsten Weinrich",
                    // City = "Redmond",
                    Email = "thorsten@example.com",
                    Status = PersonStatus.VIP,
                    CreatorId = adminID
                },
             new Person
             {
                 Name = "Yuhong Li",
                //  City = "Redmond",
                 Email = "yuhong@example.com",
                 Status = PersonStatus.Hidden,
                 CreatorId = adminID
             },
             new Person
             {
                 Name = "Jon Orton",
                 Email = "jon@example.com",
                 Status = PersonStatus.Visible,
                 CreatorId = adminID
             },
             new Person
             {
                 Name = "Diliana Alexieva-Bosseva",
                 Email = "diliana@example.com",
                 CreatorId = adminID
             }
             );
            context.SaveChanges();
        }
    }
}
