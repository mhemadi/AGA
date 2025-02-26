using AltaGasAssignment.Shared.DTOs;
using AltaGasAssignment.WebApi.Application.Services;
using AltaGasAssignment.WebApi.Application.Validator;
using AltaGasAssignment.WebApi.Data;
using FluentValidation;
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

            //TODO: Consider using interfaces
            services.AddScoped<FileUploadService>();
            services.AddScoped<EquipmentEventProcessor>();
            services.AddScoped<TripService>();

            services.AddScoped<IValidator<TripListRequestDto>, TripListRequestDtoValidator>();

            return services;
        }
    }
}
