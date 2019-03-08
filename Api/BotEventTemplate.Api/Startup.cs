﻿using System;
using System.IO;
using BotEventManagement.Services.Interfaces;
using BotEventManagement.Services.Model.Database;
using BotEventManagement.Services.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using BotEventManagement.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace BotEventTemplate.Api
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        private readonly string _statusEndpoint = "/status";

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="environment"></param>
        public Startup(IHostingEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables().Build();

        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<BotEventManagementContext>(options => options.UseSqlServer(Configuration["DefaultConnection"]));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Bot Event Management",
                    Version = "v1",
                    Description = "API to manage events",
                });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "BotEventManagement.Api.xml");

                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventParticipantService, EventParticipantsService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IUserTalksService, UserTalksService>();
            services.AddScoped<ISpeakerService, SpeakerService>();

            services.AddHealthChecks().AddSqlServer(Configuration["DefaultConnection"]);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders();

            if (!string.IsNullOrEmpty(Configuration["BasePath"]))
                app.UsePathBase(Configuration["BasePath"]);

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                 .WriteTo.Console()
                 .CreateLogger();

            app.Use(async (context, next) =>
            {
                logger.Information("Log Requisition Informations!!");

                // Request method, scheme, and path
                logger.Information("Request Method: {METHOD}", context.Request.Method);
                logger.Information("Request Scheme: {SCHEME}", context.Request.Scheme);
                logger.Information("Request Path: {PATH}", context.Request.Path);
                logger.Information("Request Path Base: {PATHBASE}", context.Request.PathBase);
                // Headers
                foreach (var header in context.Request.Headers)
                    logger.Information("Header: {KEY}: {VALUE}", header.Key, header.Value);

                // Connection: RemoteIp
                logger.Information("Request RemoteIp: {REMOTE_IP_ADDRESS}",
                    context.Connection.RemoteIpAddress);

                await next();
            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            UpdateDatabase(app);

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Bot Event Management V1");
                c.RoutePrefix = "";
            });

            app.UseSwagger();

            app.UseHealthChecks(_statusEndpoint, new HealthCheckOptions()
            {
                ResponseWriter = WriteResponse
            });

            app.UseMvc();
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<BotEventManagementContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
