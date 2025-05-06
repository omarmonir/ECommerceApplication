using eCommerceApp.Application.DependencyInjection;
using eCommerceApp.Infrastructure.DependencyInjection;
using Serilog;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();
Log.Logger.Information("Application is buliding............");
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddCors(builder =>
{
	builder.AddDefaultPolicy(options =>
	{
		options.AllowAnyHeader()
		.AllowAnyMethod()
		.WithOrigins("https://localhost:7202")
		.AllowCredentials();
	});
});
try
{
	var app = builder.Build();
	app.UseCors();
	app.UseSerilogRequestLogging();
	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseInfrastructureService();
	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();
	Log.Logger.Information("Application is running............");
	app.Run();
}catch(Exception ex)
{
	Log.Logger.Error(ex, "Application Filled to start......");
}
finally
{
	Log.CloseAndFlush();
}
