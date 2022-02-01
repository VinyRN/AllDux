using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using alldux_plataforma.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using alldux_plataforma.Services;
using Microsoft.AspNetCore.Http.Features;

namespace alldux_plataforma
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Azure")));
            //services.AddDbContext<ContentDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AzureCopia")));

            services.AddDbContext<ContentDbContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("Azure"), 
                    opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
            
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 209715200;
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, AuthMessageSender>();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                //options.Cookie.Name = ".AspNetCore.Identity.Application";
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //options.SlidingExpiration = true;
            });

            services.AddLocalization();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
                })
                 .AddErrorDescriber<CustomIdentityErrorDescriber>()
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();

            //services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUsernameEmailPolicy>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthorization(options => {
                options.AddPolicy("PadronizacaoInsumos", policy => policy.RequireClaim("Content_PadronizacaoInsumos", "True"));
                options.AddPolicy("Negociacoes", policy => policy.RequireClaim("Content_Negociacoes", "True"));
                options.AddPolicy("DiretrizesPrecificadas", policy => policy.RequireClaim("Content_DiretrizesPrecificadas", "True"));
                options.AddPolicy("DiretrizesClinicas", policy => policy.RequireClaim("Content_DiretrizesClinicas", "True"));
                options.AddPolicy("FerramentasAnalises", policy => policy.RequireClaim("Content_FerramentasAnalises", "True"));
            });

            services.Configure<DataProtectionTokenProviderOptions>(
                options => options.TokenLifespan = TimeSpan.FromHours(3)
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            var supportedCultures = new[]{ new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                FallBackToParentCultures= false
            });
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

            app.UseHttpsRedirection();
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
