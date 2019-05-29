using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using REST.Api.Entities;
using REST.Api.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http;

namespace REST.Api
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Configuration["connectionStrings:BeregnungsDBConnectionString"];
            services.AddDbContext<BeregnungsContext>(opt =>  opt.UseSqlServer(connectionString));            

            services.AddScoped<IBeregnungsRepository, BeregnungsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,BeregnungsContext beregnungsContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");

                    });
                });
            }
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Schlag, Models.SchlagDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.Name}"));
                cfg.CreateMap<Models.SchlagForCreationDto, Entities.Schlag>();

            });


            beregnungsContext.DataForContext();
            app.UseMvc();
        }
    }
}
