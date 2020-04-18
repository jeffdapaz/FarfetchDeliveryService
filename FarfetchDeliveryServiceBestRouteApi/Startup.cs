using FarfetchDeliveryServiceBestRouteApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace FarfetchDeliveryServiceBestRouteApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(options =>
            {
                string applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
                string applicationName = PlatformServices.Default.Application.ApplicationName;
                string docPath = Path.Combine(applicationPath, $"{ applicationName}.xml");

                options.IncludeXmlComments(docPath);

                options.SwaggerDoc("beta", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Farfetch Delivery Service Exercice - Best Route API",
                    Version = "beta",
                    Description = "API responsible to get the best route for delivery",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Jefferson Pires",
                        Email = "jeffdpaz@gmail.com",
                        Url = new System.Uri("https://www.linkedin.com/in/jefferson-d-3b33063b/")
                    }
                });
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(CustomExceptionFilter));
            });

            DependencyInjection.Configure(Configuration, services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = "swagger";
                setup.SwaggerEndpoint("/swagger/beta/swagger.json", "API Catalog");
            });
        }
    }
}
