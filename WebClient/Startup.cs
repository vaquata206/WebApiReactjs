using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Extensions;
using WebClient.Extentions;
using WebClient.Repositories.Implements;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Implements;
using WebClient.Services.Interfaces;

namespace WebClient
{
    /// <summary>
    /// Start up class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The IHosting Evironment</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(string.Format("appsettings.{0}.json", env.EnvironmentName), optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();

            WebConfig.ApiSystemUrl = this.Configuration.GetSection("ApiSystem").Value;
            WebConfig.ConnectionString = this.Configuration.GetSection("ConnectionString").Value;
            WebConfig.WebRootPath = env.WebRootPath;
            WebConfig.JWTKey = this.Configuration.GetSection("Jwt:Key").Value;
            WebConfig.Applications = this.Configuration.GetSection("Applications").Get<List<Application>>();
            WebConfig.RabbitMQ = this.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();
        }

        /// <summary>
        /// Application container
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// The configuration root
        /// </summary>
        public IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // options.Events = new JwtBearerEvents
                // {
                //    OnTokenValidated = context =>
                //    {
                //        return Task.CompletedTask;
                //    }
                // };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebConfig.JWTKey))
                };
            });
            services.AddHttpContextAccessor();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSingleton<IRabbitMQService, RabbitMQService>();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<AuthHelper>().SingleInstance();

            // Resgister Services
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<FeatureService>().As<IFeatureService>();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>();
            builder.RegisterType<PermissionService>().As<IPermissionService>();
            builder.RegisterType<PermissionFeatureService>().As<IPermissionFeatureService>();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>();
            builder.RegisterType<EmployeePermissionService>().As<IEmployeePermissionService>();
            builder.RegisterType<AppService>().As<IAppService>();

            // Register Repositories
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<FeatureRepository>().As<IFeatureRepository>();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>();
            builder.RegisterType<PermissionFeatureRepository>().As<IPermissionFeatureRepository>();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            builder.RegisterType<EmployeePermissionRepository>().As<IEmployeePermissionRepository>();
            builder.RegisterType<AppRepository>().As<IAppRepository>();

            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hostring environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
