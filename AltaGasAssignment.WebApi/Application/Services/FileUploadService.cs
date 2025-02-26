using AltaGasAssignment.WebApi.Data;

namespace AltaGasAssignment.WebApi.Application.Services
{
    public class FileUploadService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(AppDbContext dbContext, ILogger<FileUploadService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        //TODO: Store the file in a file storage
        public async Task<(bool Success, string Message)> SaveFile(IFormFile file, Guid fileId)
        {
            //TODO: Maybe read from config or a constant
            var uploadsFolder = @"Data\Uploads";
          
            try
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileId.ToString());
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Just to keep track of file uploads
                _dbContext.FileUploads.Add(new Data.Entities.FileUpload()
                {
                    FileName = file.FileName,
                    FileUploadType = Data.Enums.FileUploadType.EquipmentEvent,
                    Id = fileId,
                    UploadDate = DateTime.UtcNow
                });
                
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed saving the upload file");
                return (false, "Failed to store the file");
            }

            return (true, "Success");
        }
    }
}
