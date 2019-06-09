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
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            });

            var connectionString = Configuration["connectionStrings:BeregnungsDBConnectionString"];
            services.AddDbContext<BeregnungsContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<ISchlagRepository, SchlagRepository>();
            services.AddScoped<IBeregnungsRepository, BeregnungsRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(
                implementationFactory =>
                {
                    var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                    return new UrlHelper(actionContext);
                });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, BeregnungsContext beregnungsContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Information);

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
                        var exceotionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceotionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Globaler Exception logger");
                            logger.LogError(500, exceotionHandlerFeature.Error, exceotionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");

                    });
                });
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.BeregnungsDaten, Models.BeregnungsDatenDto>()
                    .ForMember(dest => dest.StartDatum, opt => opt.MapFrom(src =>
                    $"{src.StartDatum}"));
                cfg.CreateMap<Models.BeregnungsDatenForCreationDto, Entities.BeregnungsDaten>();

                cfg.CreateMap<Models.BeregnungsDatenForUpdateDto, Entities.BeregnungsDaten>();
                cfg.CreateMap<Entities.BeregnungsDaten, Models.BeregnungsDatenForUpdateDto>();

                cfg.CreateMap<Entities.Schlag, Models.SchlagDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.Name}"));
                cfg.CreateMap<Models.SchlagForCreationDto, Entities.Schlag>();

                cfg.CreateMap<Models.SchlagForUpdateDto, Entities.Schlag>();
                cfg.CreateMap<Entities.Schlag, Models.SchlagForUpdateDto>();

            });

            //DbReset
            // beregnungsContext.DataForContext();

            app.UseMvc();
        }
    }
}
