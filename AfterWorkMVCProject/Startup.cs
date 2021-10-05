using AfterWorkMVCProject.Hubs;
using AfterWorkMVCProject.Models;
using AfterWorkMVCProject.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject
{
    public class Startup
    {
        readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<AfterWorkService>();
            services.AddTransient<AccountService>();
            services.AddTransient<DigiKaljaService>();
            
            var connString = configuration.GetConnectionString("DefaultConnection");
            //var connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AfterWorkDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddControllersWithViews();
            services.AddSignalR();

            services.AddDbContext<MyContext>(o =>
            o.UseSqlServer(connString));

            services.AddDbContext<MyIdentityContext>(o =>
            o.UseSqlServer(connString));
            services.AddIdentity<MyIdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MyIdentityContext>()
            .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();
            services.ConfigureApplicationCookie(
            o => o.LoginPath = "/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
