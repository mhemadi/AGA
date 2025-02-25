using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Data.Configuration
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.CityId).IsUnique();

            builder.Property(x => x.Name).HasColumnType("varchar(100)"); //Not that important
            builder.Property(x => x.TimeZoneStr).HasColumnType("varchar(100)");
        }
    }
}
