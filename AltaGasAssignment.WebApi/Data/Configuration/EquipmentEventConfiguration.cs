using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Data.Configuration
{
    public class EquipmentEventConfiguration : IEntityTypeConfiguration<EquipmentEvent>
    {
        public void Configure(EntityTypeBuilder<EquipmentEvent> builder)
        {
            builder.HasKey(x => x.Id);

            //I prefer to be explicit about the relationships
            builder.HasOne(x => x.Trip).WithMany(t => t.EquipmentEvents).HasForeignKey(x => x.TripId);
            builder.HasOne(x => x.EquipmentEventType).WithMany().HasForeignKey(x => x.EquipmentEventTypeId);
            builder.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);
        }
    }
}
