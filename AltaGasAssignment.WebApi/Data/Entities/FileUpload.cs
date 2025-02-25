using AltaGasAssignment.WebApi.Data.Enums;

namespace AltaGasAssignment.WebApi.Data.Entities
{
    public class FileUpload
    {
        //TODO: Store User Id
        public Guid Id { get; set; }
        public DateTime UploadDate { get; set; }
        public FileUploadType FileUploadType { get; set; }
        public required string FileName { get; set; }
    }
}
