using Human_Resources_Management.ENTITIES.Entities;
using Human_Resources_Management.REPOSITORIES;
using Human_Resources_Management.REPOSITORIES.Abstract;
using Human_Resources_Management.REPOSITORIES.Concrete;
using Human_Resources_Management.REPOSITORIES.Context;
using HumanResourcesManagement.BUSINESS.Abstract;
using HumanResourcesManagement.BUSINESS.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResourcesManagement.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HumanResourcesManagement.API", Version = "v1" });
            });
           
            services.AddDbContext<HRMDbContext>(options =>
            {
                options.UseSqlServer("Server=tcp:iamhere.database.windows.net,1433;Initial Catalog=KD11ik;Persist Security Info=False;User ID=iamhere;Password=ik-123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            });
        }

        
            

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HumanResourcesManagement.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            SeedData.Seed(app);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
