using Microsoft.Extensions.DependencyInjection;
using System;

namespace TODO.IOC
{
    public class ServiceContainer
    {
        public static void Register(IServiceCollection services)
        {
            //services.AddScoped<IUserService, UserService>();
            //services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

            //var serviceProvider = services.BuildServiceProvider();
            //var dbConnection = serviceProvider.GetService<IDatabaseConnection>();

            //services.AddDbContext<RBCContext>(options => options.UseSqlServer(dbConnection.GetConnection()));
        }
    }
}
