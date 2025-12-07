using Course.DAL.DataBase;
using Course.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Course.DAL.Utilityes
{
    public class SeedData : ISeedData
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SeedData (ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context=context;
            this.userManager=userManager;
            this.roleManager=roleManager;
        }
        public async Task DataSeed ()
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
            if (!await context.Users.AnyAsync())
            {
                await SeedUsers();
                await context.SaveChangesAsync();
            }
            if (!await context.Categories.AnyAsync())
            {
                await SeedCategories();
                await context.SaveChangesAsync();
            }
            if (!await context.Courses.AnyAsync())
            {
                await SeedCourses();
                await context.SaveChangesAsync();
            }

        }
        public async Task SeedCategories ()
        {
            await context.Categories.AddRangeAsync(new List<Category>
            {
                new Category { Name = "Programming", Description = "Courses related to programming languages and software development.", ImgeUrl = "Programming.jpg" },
                new Category {Name = "Scince", Description = "Courses covering various scientific disciplines." , ImgeUrl = "Scince.jpg" },
                new Category {Name = "Mathematics", Description = "Courses focused on mathematical concepts and applications.", ImgeUrl = "Mathematics.png" },
                new Category {Name = "Arts", Description = "Courses exploring various forms of art and creativity.", ImgeUrl = "Arts.jpg" },
                new Category {Name = "History", Description = "Courses delving into historical events and periods." , ImgeUrl = "History.jpg" },
                new Category {Name = "Languages", Description = "Courses for learning new languages and improving language skills." , ImgeUrl = "Languages.jpg" },
                new Category {Name = "Business", Description = "Courses on business management, entrepreneurship, and finance.", ImgeUrl = "Business.jpg" }
           });
        }

        public async Task SeedCourses ()
        {
            var instructor = await userManager.Users.FirstOrDefaultAsync(u => u.UserName=="Teatcher");
            var categories = await context.Categories.ToListAsync();

            await context.Courses.AddRangeAsync(new List<Course.DAL.Models.Course>
            {
                new Course.DAL.Models.Course
                {
                    Title = "Introduction to C# Programming",
                    Description = "Learn the basics of C# programming language.",
                    Price = 49.99m,
                    CategoryId = categories.First(c => c.Name == "Programming").Id,
                    InstructorId = instructor.Id,
                    ImgeUrl= "Course.jpg"
                },
                new Course.DAL.Models.Course
                {
                    Title = "Basic Physics",
                    Description = "Understand the fundamental concepts of physics.",
                    Price = 39.99m,
                    CategoryId = categories.First(c=> c.Name == "Scince").Id,
                    InstructorId = instructor.Id,
                    ImgeUrl = "Course.jpg"
                },
                new Course.DAL.Models.Course
                {
                    Title = "Calculus I",
                    Description = "An introduction to differential and integral calculus.",
                    Price = 59.99m,
                    CategoryId = categories.First(c=> c.Name == "Mathematics").Id,
                   InstructorId = instructor.Id,
                   ImgeUrl = "Course.jpg"
                },
                new Course.DAL.Models.Course
                {
                    Title = "Digital Painting",
                    Description = "Learn techniques for creating digital artwork.",
                    Price = 29.99m,
                     CategoryId = categories.First(c=> c.Name == "Arts").Id,
                     InstructorId = instructor.Id,
                      ImgeUrl = "Course.jpg"
                },
                new Course.DAL.Models.Course
                {
                    Title = "World History",
                    Description = "Explore significant events in world history.",
                    Price = 44.99m,
                    CategoryId = categories.First(c=> c.Name == "History").Id,
                    InstructorId= instructor.Id,
                    ImgeUrl = "Course.jpg"
                },
                new Course.DAL.Models.Course
                {
                    Title = "Spanish for Beginners",
                    Description = "Start speaking Spanish with this beginner course.",
                    Price = 34.99m,
                     CategoryId = categories.First(c=> c.Name == "Languages").Id,
                    InstructorId = instructor.Id,
                    ImgeUrl = "Course.jpg"
                },
                new Course.DAL.Models.Course
                {
                    Title = "Business Management Basics",
                    Description = "Learn the fundamentals of managing a business.",
                    Price = 54.99m,
                    CategoryId = categories.First(c=> c.Name == "Business").Id,
                    InstructorId = instructor.Id,
                    ImgeUrl = "Course.jpg"
                }
            });
        }

        public async Task SeedUsers ()
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await roleManager.CreateAsync(new IdentityRole("Instructor"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            var users = new List<ApplicationUser>
            {
            new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = "superAdmin@gmail.com",
                      FullName = "Super Admin",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Address = "123 Admin St",
                    City = "Admin City",
                    Country = "Adminland",
                    EmailConfirmed = true,
                },
                new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin12@gmail.com",
                    FullName = "Admin User",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Address = "456 Admin Ave",
                    City = "Adminville",
                    Country = "Adminland",
                    EmailConfirmed = true,
                },
              new ApplicationUser
              {
                  UserName = "Teatcher",
                  Email = "teatchermoh@gmail.com",
                    FullName = "Teatcher User",
                    DateOfBirth = new DateTime(1985, 3, 20),
                    Address = "789 Teatcher Rd",
                    City = "Teatcher City",
                    Country = "Teatcherland",
                    EmailConfirmed = true
              },
              new ApplicationUser
              {
                    UserName = "Student",
                    Email = "student@gmail.com",
                    FullName = "Student User",
                    DateOfBirth = new DateTime(2000, 7, 10),
                    Address = "101 Student Blvd",
                    City = "Student City",
                    Country = "Studentland",
                    EmailConfirmed = true
              }
            };
            await userManager.CreateAsync(users[0], "123@SuperAdmin");
            await userManager.CreateAsync(users[1], "123@Admin12");
            await userManager.CreateAsync(users[2], "123@Teatcher");
            await userManager.CreateAsync(users[3], "123@Student");
            await userManager.AddToRoleAsync(users[0], Roles.SuperAdmin.ToString());
            await userManager.AddToRoleAsync(users[1], Roles.Admin.ToString());
            await userManager.AddToRoleAsync(users[2], Roles.Instructor.ToString());
            await userManager.AddToRoleAsync(users[3], Roles.User.ToString());
        }

    }
}

