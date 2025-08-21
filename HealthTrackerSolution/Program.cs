using HealthTrackerSolution.DataContext;
using HealthTrackerSolution.Interface;
using HealthTrackerSolution.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Get connection string from config
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Check if Render provides DATABASE_URL (Postgres)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    // Render gives DATABASE_URL in the format:
    // postgres://username:password@host:port/dbname
    var dbUri = new Uri(databaseUrl);
    var userInfo = dbUri.UserInfo.Split(':');

    connectionString =
        $"Host={dbUri.Host};Port={dbUri.Port};Database={dbUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

    // Use Postgres when running on Render
    builder.Services.AddDbContext<dataContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    // Default to SQL Server locally
    builder.Services.AddDbContext<dataContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddScoped<IUser, UserRepository>();

var app = builder.Build();

// Use CORS middleware
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
