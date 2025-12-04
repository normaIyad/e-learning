using Course.Bll.Service.Class;
using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DataBase;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Course.DAL.Repositry.Class;
using Course.DAL.Utilityes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using Scalar.AspNetCore;
using Stripe;
using System.Text;

namespace Course
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --------------------
            // Database Configuration
            // --------------------
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            // Force encryption and trust certificate for hosted SQL Server
            connection+=";Encrypt=True;TrustServerCertificate=True;";

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connection,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 0, // temporarily disable retries to catch real errors
                        maxRetryDelay: TimeSpan.FromSeconds(0),
                        errorNumbersToAdd: null
                    )
                )
            );

            // --------------------
            // Stripe Configuration
            // --------------------
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey=builder.Configuration["Stripe:SecretKey"];

            // --------------------
            // Controllers & API
            // --------------------
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            // --------------------
            // Repositories
            // --------------------
            builder.Services.AddScoped(typeof(IGenralRepositry<>), typeof(GenralRepositry<>));
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<ICourseRepositry, CourseRepositry>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<IEnrollmentsRepo, EnrollmentsRepo>();
            builder.Services.AddScoped<ICourseMaterialRepo, CourseMaterialRepo>();
            builder.Services.AddScoped<IExamRepositry, ExamRepositry>();
            builder.Services.AddScoped<IQuestionRepositry, QuestionRepositry>();
            builder.Services.AddScoped<IQuestionOptionRepositry, QuestionOptionRepositry>();
            builder.Services.AddScoped<IStudentAnswersRepo, StudentAnswersRepo>();
            builder.Services.AddScoped<IExamResultRepo, ExamResultRepo>();
            builder.Services.AddScoped<IUserRepositry, UserRepositry>();

            // --------------------
            // Services
            // --------------------
            builder.Services.AddScoped(typeof(IGeneralService<,,>), typeof(GeneralService<,,>));
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IAuthentication, Authentication>();
            builder.Services.AddScoped<IPaymentService, PaymentServices>();
            builder.Services.AddScoped<ICourseMaterialService, CourseMaterialService>();
            builder.Services.AddScoped<IFileService, Bll.Service.GenralIService.FileService>();
            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IStudentAnswersService, StudentAnswersService>();
            builder.Services.AddScoped<IExamResultRepo, ExamResultRepo>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<Repoer>();

            // --------------------
            // PDF License
            // --------------------
            QuestPDF.Settings.License=LicenseType.Community;

            // --------------------
            // Identity & Seed Data
            // --------------------
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<ISeedData, SeedData>();

            // --------------------
            // JWT Authentication
            // --------------------
            var key = builder.Configuration["JWT:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(key);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters=new TokenValidationParameters
                {
                    ValidateIssuer=false,
                    ValidateAudience=false,
                    ValidateLifetime=true,
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(keyBytes),
                    RoleClaimType="http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                };
            });

            // --------------------
            // Authorization
            // --------------------
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserRole", policy => policy.RequireRole("User"));
                options.AddPolicy("AdminRole", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            // --------------------
            // Apply migrations & seed data
            // --------------------
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    Console.WriteLine("Checking pending migrations...");
                    var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                        Console.WriteLine("Applying migrations...");
                        await dbContext.Database.MigrateAsync();
                        Console.WriteLine("Migrations applied successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No pending migrations.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database migration failed: "+ex);
                    throw;
                }

                var seeder = scope.ServiceProvider.GetRequiredService<ISeedData>();
                await seeder.DataSeed();
            }

            // --------------------
            // Middleware
            // --------------------
            //if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
            //{
            //    app.MapOpenApi();
            //    app.MapScalarApiReference();
            //}
            app.MapOpenApi();                 // enable OpenAPI in all environments
            app.MapScalarApiReference();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
