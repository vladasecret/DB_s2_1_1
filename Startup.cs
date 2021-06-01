using DB_s2_1_1.EntityModels;
using DB_s2_1_1.Services;
using FloatingDecimalBinderModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1
{
    public class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }

        public static readonly ILoggerFactory dbContextLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });*/

            services.AddDbContext<TrainsContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                if (HostingEnvironment.IsDevelopment()) {
                    options.EnableSensitiveDataLogging();
                    options.UseLoggerFactory(dbContextLoggerFactory);
                }
                });

            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IStationsService, StationsService>();
            services.AddScoped<IRoutesService, RoutesService>();
            services.AddScoped<ITrainsService, TrainsService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<ITimetablesService, TimetablesService>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddScoped<IManualRawSqlService, ManualRawSqlService>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddMvc(opts =>
            {
                opts.ModelBinderProviders.Insert(0, new CustomDecimalModelBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

        }

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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
