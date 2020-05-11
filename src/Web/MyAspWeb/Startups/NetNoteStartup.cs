﻿using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyAspWeb.Contexts;
using MyAspWeb.Repositories;

namespace MyAspWeb.Startups
{
    public class NetNoteStartup
    {
        public NetNoteStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(ops =>
            {
                ops.EnableEndpointRouting = false;
            });
            services.AddControllersWithViews();

            var connection = "Data Source=./note.db";
            //connection = @"Server=.;Database=Note;UID=sa;PWD=sa;";
            services.AddDbContext<NoteContext>(options =>
            {
                options.UseSqlite(connection);
                //options.UseSqlServer(connection);
            });

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteTypeRepository, NoteTypeRepository>();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //解决日志显示中文乱码问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "note",
                //    template: "note/{action}",
                //    defaults: new
                //    {
                //        action = "index",
                //    });
                //routes.MapRoute(
                //    name: "html",
                //    template: "html",
                //    "~/wwwroot/WebSocketClient.html");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=index}/{id?}"
                    );
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=index}"
                    );
            });
        }
    }
}