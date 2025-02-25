using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltaGasAssignment.WebApi.Data.Configuration
{
    public class FileUploadConfiguration
    {
        public void Configure(EntityTypeBuilder<FileUpload> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x=>x.FileName).HasColumnType("varchar(100)");
        }
    }
}
