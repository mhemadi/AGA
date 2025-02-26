using AltaGasAssignment.WebApi.Application;
using AltaGasAssignment.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapFileUploadEndpoints();
app.MapTripEndpoints();
app.MapInitializeEndpoints();

app.Run();
