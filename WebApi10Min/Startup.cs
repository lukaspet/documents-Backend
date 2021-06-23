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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi10Min.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Http;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WebApi10Min.Helpers.Schedulers;
using WebApi10Min.Helpers.Email;

namespace WebApi10Min
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
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            // Add our job
            services.AddSingleton<EmailJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(EmailJob),
                cronExpression: "0 0 8 ? * MON")); // run every week on monday at 8:00
                // cronExpression: "0/5 * * * * ?"));

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            services.AddDbContext<MyDbContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AuthDbContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("DefaultConnection1")));

            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<Aspnetusers, Aspnetroles>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultUI();

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        //ValidIssuer = "http://localhost:59695/",
                        ValidIssuer = "http://10.1.100.50:59695/",
                        ClockSkew = TimeSpan.Zero,
                        //ValidAudience = "http://localhost:59695/",
                        ValidAudience = "http://10.1.100.50:59695/",
                    };
                });
            services.AddCors(options => { options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("X-Pagination")); });
            //services.AddDbContext<DocumentContext>(opt =>
            //   opt.UseInMemoryDatabase("DocumentList"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
                app.UseHsts();
            }
            //app.Use(async (context, next) =>   // not found exception
            //{
            //    await next();
            //    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
            //    {
            //        context.Request.Path = "/index.html";
            //        await next();
            //    }
            //});
            app.UseCors("AllowAllOrigins");
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseDefaultFiles(); // angular wwwroot
            app.UseStaticFiles(); // angular wwwroot
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseMvc();          
        }
    }
}
