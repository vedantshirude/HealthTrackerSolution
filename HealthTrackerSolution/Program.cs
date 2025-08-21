using HealthTrackerSolution.DataContext;
using HealthTrackerSolution.Interface;
using HealthTrackerSolution.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔑 Prefer DATABASE_URL (Render), fallback to appsettings.json
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Convert DATABASE_URL to Npgsql connection string
    var dbUri = new Uri(databaseUrl);
    var userInfo = dbUri.UserInfo.Split(':');

    connectionString =
        $"Host={dbUri.Host};Port={dbUri.Port};Database={dbUri.AbsolutePath.TrimStart('/')};" +
        $"Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

    builder.Services.AddDbContext<dataContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    // fallback for local SQL Server
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<dataContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddScoped<IUser, UserRepository>();

var app = builder.Build();

// Use CORS middleware
app.UseCors("AllowAll");

// Swagger only in dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
