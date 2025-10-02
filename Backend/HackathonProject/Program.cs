using Microsoft.EntityFrameworkCore;
using HackathonProject.Models; // Assuming DataContext, Student, and Course are here

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configure Services ---

// 1.1 Add Data Context (using SQLite)
// NOTE: This setup requires the Microsoft.EntityFrameworkCore.Sqlite NuGet package.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 1.2 Add Controllers and OpenAPI/Swagger for testing
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <-- RESTORED

// 1.3 Configure CORS (Crucial for allowing your Next.js frontend to access this API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            // In development, allow all origins. In production, restrict this.
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- 2. Configure HTTP Request Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // <-- RESTORED
    app.UseSwaggerUI(); // <-- RESTORED

    // 2.1 Automatically apply migrations on startup in Development
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataContext>();

        // This command ensures the database is created and migrations are applied.
        context.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy"); // Use the defined CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
