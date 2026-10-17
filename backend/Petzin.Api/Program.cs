using Microsoft.EntityFrameworkCore;
using Petzin.Api.Data;
using Petzin.Api.Erros;
using Petzin.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<TutorService>();
builder.Services.AddScoped<PetService>();
builder.Services.AddScoped<CanilService>();
builder.Services.AddScoped<ReservaService>();

var app = builder.Build();

app.UseMiddleware<ErroMiddleware>();
app.MapControllers();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    for (var tentativa = 1; tentativa <= 10; tentativa++)
    {
        try
        {
            db.Database.Migrate();
            DbSeeder.Seed(db);
            break;
        }
        catch
        {
            if (tentativa == 10)
            {
                throw;
            }

            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}

app.Run();
