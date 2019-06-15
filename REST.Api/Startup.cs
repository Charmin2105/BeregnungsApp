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
using System.Reflection;
using System.IO;

namespace REST.Api
{
    public class Startup
    {
        private string swaggername = "BeregnungsOpenApiSpecification";

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

                var jsonOutputFormatter = setupAction.OutputFormatters
                    .OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.ostfalia.hateoas+json");
                }

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
            services.AddScoped<IBetriebRepository, BetriebRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(
                implementationFactory =>
                {
                    var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                    return new UrlHelper(actionContext);
                });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();
            services.AddHttpCacheHeaders(
                (expirationModelOptions)
                =>
                {
                    expirationModelOptions.MaxAge = 600;
                },
                (validationModelOptions)
                =>
                {
                    validationModelOptions.MustRevalidate = true;
                });
            services.AddResponseCaching();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(swaggername,
                    new Swashbuckle.AspNetCore.Swagger.Info()
                    {
                        Title = "Beregnungs Api",
                        Version = "1",
                        Description = "Mit dieser Api können Beregnungsdaten, Schläge, Betriebe " +
                        "und Mitarbeiter eines Betirebs innerhalb eines Beregnungsvereins erstellt," +
                        " geändert und gelöscht werden. Diese Api wurde im Rahmen einer Bachelor " +
                        "Arbeit erstellt.",
                        Contact = new Swashbuckle.AspNetCore.Swagger.Contact()
                        {
                            Name = "Nico Herrmann",
                            Email = "ni.herrmann@ostfalia.de"                           
                        }
                    });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });


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
                //Mapper Beregnungsdaten
                cfg.CreateMap<Entities.BeregnungsDaten, Models.BeregnungsDatenDto>()
                    .ForMember(dest => dest.StartDatum, opt => opt.MapFrom(src =>
                    $"{src.StartDatum}"));
                cfg.CreateMap<Models.BeregnungsDatenForCreationDto, Entities.BeregnungsDaten>();

                cfg.CreateMap<Models.BeregnungsDatenForUpdateDto, Entities.BeregnungsDaten>();
                cfg.CreateMap<Entities.BeregnungsDaten, Models.BeregnungsDatenForUpdateDto>();

                //Maper Schlag
                cfg.CreateMap<Entities.Schlag, Models.SchlagDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.Name}"));
                cfg.CreateMap<Models.SchlagForCreationDto, Entities.Schlag>();

                cfg.CreateMap<Models.SchlagForUpdateDto, Entities.Schlag>();
                cfg.CreateMap<Entities.Schlag, Models.SchlagForUpdateDto>();

                //Mapper Betrieb
                cfg.CreateMap<Entities.Betrieb, Models.BetriebDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.Name}"));
                cfg.CreateMap<Models.BetriebForCreationDto, Entities.Betrieb>();

                cfg.CreateMap<Models.BetriebForUpdateDto, Entities.Betrieb>();
                cfg.CreateMap<Entities.Betrieb, Models.BetriebForUpdateDto>();

                //Mapper Mitarbeiter
                cfg.CreateMap<Entities.Mitarbeiter, Models.MitarbeiterDto>()
                    .ForMember(dest => dest.Nachname, opt => opt.MapFrom(src =>
                    $"{src.Nachname}"));
                cfg.CreateMap<Models.MitarbeiterForCreationDto, Entities.Mitarbeiter>();

                cfg.CreateMap<Models.MitarbeiterForUpdateDto, Entities.Mitarbeiter>();
                cfg.CreateMap<Entities.Mitarbeiter, Models.MitarbeiterForUpdateDto>();
            });

            //DbReset
            //beregnungsContext.DataForContext();

            //app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint($"/swagger/{swaggername}/swagger.json",
                    "Beregnungs Api");
            });

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();


            app.UseMvc();
        }
    }
}
