using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using ValueTech.Data;
using ValueTech.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
builder.Services.AddDataLayer(connectionString);


builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IComunaService, ComunaService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "ValueTech API v1");
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<ValueTech.Api.Middlewares.GlobalExceptionMiddleware>();

app.UseRateLimiter(); // Activar Rate Limiting
app.UseMiddleware<ValueTech.Api.Middlewares.ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers()
   .RequireRateLimiting("fixed"); // Aplicar politica globalmente o por controlador

app.Run();
