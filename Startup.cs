using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Security;
using AutoGraphic.Models.ViewModels;
using AutoGraphic.Controllers;
using AutoGraphic.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AutoGraphic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services )
        {
            services.AddControllersWithViews();
            //binding Database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("constr"));
            });

            //add identity
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            //services.AddMailKit(options => options.UseMailKit(Configuration.GetSection("EmailSender").Get<MailKitOptions>()));
            //services.AddTransient<Models.IEmailSender, EmailSender>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Departments", policy =>
            //    {
            //        policy.RequireClaim("Departments");
            //    });
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminRole", policy =>
            //        policy.RequireClaim("Admin"));
            //    options.AddPolicy("UserRole", policy =>
            //        policy.RequireClaim("User"));
            //    options.AddPolicy("AdminRole,UserRole", policy =>
            //        policy.RequireAssertion(context =>
            //            context.User.HasClaim(c =>
            //                (c.Type == "Admin" && c.Value == "Admin") ||
            //                (c.Type == "User" && c.Value == "User"))
            //        ));
            //});

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin");
                });

                options.AddPolicy("UserOnly", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("User");
                });
                options.AddPolicy("UserAndAdmin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("User", "Admin");
                });
                options.AddPolicy("Departments", policy =>
                {
                    policy.RequireClaim("Departments");
                });

            });

         


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
.AddCookie(options =>
{
    options.LoginPath = "/Account/LogIn";
    options.LogoutPath = "/Account/Logout";
   
    //options.AccessDeniedPath = "/Account/AccessDenied";
});
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(10);
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
        name: "dashboard",
        areaName: "Dashboard",
        pattern: "Dashboard/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "userdashboard",
                    areaName: "UserDashboard",
                    pattern: "UserDashboard/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Register}/{id?}");
            });
        }
    }
}
