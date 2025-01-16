using Infrastructure;
using Infrastructure.Services;
using Persistence;
using TrackerAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add configuration file
builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Services.AddScoped<IEmailService, EmailService>();
// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System
            .Text
            .Json
            .Serialization
            .ReferenceHandler
            .IgnoreCycles;
    });
;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
app.UseGlobalExceptionMiddleware();
app.UseAuthMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
