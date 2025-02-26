using AltaGasAssignment.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
