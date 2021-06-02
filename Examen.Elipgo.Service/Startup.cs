using Examen.Elipgo.DAL.Contexts;
using Examen.Elipgo.DAO.Auth;
using Examen.Elipgo.DAO.Interfaces;
using Examen.Elipgo.DAO.Mapping;
using Examen.Elipgo.DAO.Repository;
using Examen.Elipgo.DAO.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;

namespace Examen.Elipgo.Service
{
    public class Startup
    {
        private readonly string GROUP_NAME = "API";
        private readonly string VERSION = "1.0Alpha";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var IssuerSigningKey = Configuration.GetSection("AppSettings:IssuerSigningKey").Get<string>();
            var settings = Configuration.GetSection("AppIdentitySettings").Get<AppIdentitySettings>();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ExamElipgo")),
                ServiceLifetime.Transient
            );

            services.AddIdentity<IdentityUser, IdentityRole>(cfg =>
                {
                    //Password settings
                    cfg.Password.RequireDigit = settings.Password.RequireDigit;
                    cfg.Password.RequiredLength = settings.Password.RequiredLength;
                    cfg.Password.RequireNonAlphanumeric = settings.Password.RequireNonAlphanumeric;
                    cfg.Password.RequireUppercase = settings.Password.RequireUppercase;
                    cfg.Password.RequireLowercase = settings.Password.RequireLowercase;


                    //Lockout settings
                    cfg.Lockout.AllowedForNewUsers = settings.Lockout.AllowedForNewUsers;
                    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(settings.Lockout.DefaultLockoutTimeSpanInMins);
                    cfg.Lockout.MaxFailedAccessAttempts = settings.Lockout.MaxFailedAccessAttempts;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IssuerSigningKey))
                };
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }, AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                    opt.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();

            //Inject Settings
            services.Configure<AppIdentitySettings>(Configuration.GetSection("AppIdentitySettings"));

            services.AddSwaggerGen();
            AddSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    context.Database.Migrate();
            //}

            app.UseHttpsRedirection();

            //Area swagger data
            app.UseSwagger(c => c.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{GROUP_NAME}/swagger.json", "My API V1");
                c.RoutePrefix = "";
            });

            //END Area swagger data

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {

                opt.SwaggerDoc(GROUP_NAME, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"Evaluation {GROUP_NAME}",
                    Version = VERSION,
                    Description = "Service API REST Application Developed for present my skills like developer",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Julio Cesar Otero Ortiz",
                        Email = "otero.ortiz404@gmail.com",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });
        }
    }
}
