using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Data.Configuration
{
    public class EquipmentEventTypeConfiguration : IEntityTypeConfiguration<EquipmentEventType>
    {
        public void Configure(EntityTypeBuilder<EquipmentEventType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.Code).HasColumnType("varchar(10)");
            builder.Property(x => x.EventDescription).HasColumnType("varchar(100)");
            builder.Property(x => x.LongDescription).HasColumnType("text");
        }
    }
}
