using AltaGasAssignment.Shared.Constants;
using AltaGasAssignment.WebApi.Application.Services;

namespace AltaGasAssignment.WebApi.Endpoints
{
    public static class FileUploadEndpoints
    {
        public static IEndpointRouteBuilder MapFileUploadEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/uploadEquipmentEvents", async (IFormFile file,
                FileUploadService fileUploadService) =>
            {
                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest("No file uploaded.");
                }

                if (file.Length > ConstantValues.MaxFileSize)
                {
                    return Results.BadRequest("File size exceeds the limit (1MB).");
                }

                var fileId = Guid.NewGuid();
                var saveResult = await fileUploadService.SaveFile(file, fileId);
                if (saveResult.Success == false)
                {
                    return Results.BadRequest(saveResult.Message);
                }

                return Results.Ok(fileId);
            }).DisableAntiforgery(); //TODO: Need to consider security in production

            routes.MapPost("/processEquipmentEventFile/{fileId}", async (Guid fileId,
                EquipmentEventProcessor processor) =>
            {
                var loadResult = await processor.Load(fileId);
                //TODO: Not all errors are bad requests, differentiate between them
                if (loadResult.Success == false)
                    return Results.BadRequest(loadResult.Message);

                var processResult = processor.Process();
                if (processResult.Success == false)
                    return Results.BadRequest(processResult.Message);

                var saveResult = await processor.Save();
                if (saveResult.Success == false)
                    return Results.BadRequest(saveResult.Message);

                return Results.Ok();
            });

            return routes;
        }
    }
}
