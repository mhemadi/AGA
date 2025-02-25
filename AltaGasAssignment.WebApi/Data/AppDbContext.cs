using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<City> Cities => Set<City>();
        public DbSet<EquipmentEvent> EquipmentEvents => Set<EquipmentEvent>();
        public DbSet<EquipmentEventType> EquipmentEventTypes => Set<EquipmentEventType>();
        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<FileUpload> FileUploads => Set<FileUpload>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
