using AltaGasAssignment.WebApi.Application;
using AltaGasAssignment.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.MapFileUploadEndpoints();
app.MapTripEndpoints();

app.Run();
