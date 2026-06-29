using StudentService.API.Config;
using StudentService.API.Services;
using StudentService.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjection();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Run migrations and seed data
await DatabaseSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapGrpcService<StudentGrpcService>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
