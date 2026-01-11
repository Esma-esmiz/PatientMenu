using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Api.Services;
using Microsoft.OpenApi.Models;
using Api.Middleware;
using Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PatientMenu API", Version = "v1" });

    // Add the tenant header to all endpoints
    c.OperationFilter<SwaggerTenantHeader>();
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();


builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();

builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddSingleton<IDbConnectionFactory, SqlServerConnectionFactory>();
builder.Services.AddScoped<IMenuIteamRepository, MenuItemRepository>();


// Create  Sql server connection
// SQL Server connection with connection pool (default EF Core pools)
// Pooling is automatic; you can set Max Pool Size in connection string if needed
var connectionString = "Server=localhost,1433;Database=PatientMenu;User Id=sa;Password=Root1234!;TrustServerCertificate=True;Max Pool Size=5;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Apply migrations ONLY in Development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();   // automatic migration

         // Seed Tenant if not exists
        if (!db.Tenants.Any())
        {
            db.Tenants.Add(new Tenant
            {
                Name = "DefaultTenant"
            });

            db.SaveChanges();
        }
    }

}


app.UseMiddleware<TenantMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

