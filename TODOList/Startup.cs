using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TODO.Contacts.Connection;
using TODO.Contacts.Repository;
using TODO.Contacts.Services;
using TODO.Repo;
using TODO.Repo.Repository;
using TODO.Service;

namespace TODOList
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
            services.AddControllersWithViews()
                 .AddJsonOptions(options => {
                     options.JsonSerializerOptions.IgnoreNullValues = true;
                 });
            JwtConfig(services);

            // DI
            Register(services);
            ServiceSettings(services);
        }

        private void Register(IServiceCollection services)
        {
            services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

            var serviceProvider = services.BuildServiceProvider();
            var dbConnection = serviceProvider.GetService<IDatabaseConnection>();
            services.AddDbContext<ToDoDbContext>(options => options.UseSqlite(dbConnection.GetConnection()));
            services.AddScoped(typeof(IRepo<>), typeof(GenericRepo<>));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITodoService, ToDoService>();
            services.AddScoped<UserContext, UserContext>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");
            });

        }

        private void ServiceSettings(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddRouting(options => options.LowercaseUrls = true);

        }
        private void JwtConfig(IServiceCollection services)
        {
            // configure strongly typed settings objects

            var _key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings")["Secret"].ToCharArray());
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = context.Principal.Identity.Name;
                            var user = userService.GetUserByid(int.Parse(userId)).ConfigureAwait(false)
                            .GetAwaiter().GetResult();
                               
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            else
                            {
                                // set the user context.
                                var contextService = context.HttpContext.RequestServices.GetRequiredService<UserContext>();
                                contextService.UserId = int.Parse(userId);
                            }
                            
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

        }
    }
}
