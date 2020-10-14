using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetFinderCore;

namespace PetFinderWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddScoped<IPetFinderRepositoryClient, PetFinderRepositoryClient>();
            services.AddScoped<IPetFinder, PetFinder>();
            EndpointProvider ep = BuildEndpointProvider();
            services.AddSingleton<IEndpointProvider>(ep);
        }

        private EndpointProvider BuildEndpointProvider()
        {
            var ep = new EndpointProvider();
            int x = 0;
            var name = Configuration[$"Endpoints:{x}:Name"];
            var url = Configuration[$"Endpoints:{x}:Endpoint"];
            while (!string.IsNullOrWhiteSpace(name))
            {
                ep.AddEndpoint(name, url);
                x += 1;
                name = Configuration[$"Endpoints:{x}:Name"];
                url = Configuration[$"Endpoints:{x}:Endpoint"];
            }

            return ep;
        }
    }
}