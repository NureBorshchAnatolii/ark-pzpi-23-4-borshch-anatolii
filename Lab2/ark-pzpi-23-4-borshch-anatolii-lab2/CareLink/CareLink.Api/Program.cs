using CareLink.Api.SwaggerConfigs;
using CareLink.Application;
using CareLink.Persistence;
using CareLink.Security;
using QuestPDF.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

QuestPDF.Settings.License = LicenseType.Community;

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSecurityServices(configuration);

builder.Services.AddControllers();

builder.Services.AddBearerSecurityScheme();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
