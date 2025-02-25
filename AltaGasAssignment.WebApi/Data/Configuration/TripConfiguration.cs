using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Data.Configuration
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.OriginCity).WithMany().HasForeignKey(x => x.OriginCityId);
            builder.HasOne(x => x.DestinationCity).WithMany().HasForeignKey(x => x.DestinationCityId);

            builder.HasIndex(x => x.StartDate);
            builder.HasIndex(x => x.EndDate);
            builder.HasIndex(x => x.EquipmentId);
            builder.HasIndex(x => x.TotalTripMinutes);
        }
    }
}
