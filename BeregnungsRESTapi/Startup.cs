using System;
using BeregnungsRESTapi.Controllers;
using BeregnungsRESTapi.Entities;
using BeregnungsRESTapi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeregnungsRESTapi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Configuration["connectionStrings:DBConnectionString"];

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            services.AddDbContext<BeregnungsContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            services.AddScoped<IBeregnungsdatenRepository, BeregnungsdatenRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BeregnungsContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            AutoMapperSetup();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AutoMapperSetup()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Beregnungs, Models.BergnungsDto>()
                    .ForMember(dest => dest.StartDatum, opt => opt.MapFrom(src =>
                    $"{src.StartUhrzeit} {src.EndeDatum}"))
                    .ForMember(dest => dest.Duese, opt => opt.MapFrom(src =>
                    src.Schlag));

                //cfg.CreateMap<Entities., Models.BookDto>();
            });
        }
    }
}
