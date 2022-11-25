using Human_Resources_Management.REPOSITORIES;
using Human_Resources_Management.REPOSITORIES.Abstract;
using Human_Resources_Management.REPOSITORIES.Concrete;
using Human_Resources_Management.REPOSITORIES.Context;
using HumanResourcesManagement.BUSINESS.Abstract;
using HumanResourcesManagement.BUSINESS.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Human_Resources_Management.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddDbContext<HRMDbContext>(options =>
            {
                options.UseSqlServer("Server=yourserver,1433;Initial Catalog=KD11ik;Persist Security Info=False;User ID=yourid;Password=yourpass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home/Eriþim Engellendi";       //ÝF you are dont have permission u will redirect this page
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);       //Cookie timer
                options.SlidingExpiration = true;       //cookie sliding

                options.LoginPath = "/Home/Login";
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
                //options.ReturnUrlParameter=
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            SeedData.Seed(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

