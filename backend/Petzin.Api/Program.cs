using Microsoft.EntityFrameworkCore;
using Petzin.Api.Data;
using Petzin.Api.Erros;
using Petzin.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Petzin API",
        Version = "v1",
        Description = "API de gerenciamento do Petzin"
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<TutorService>();
builder.Services.AddScoped<PetService>();
builder.Services.AddScoped<CanilService>();
builder.Services.AddScoped<ReservaService>();

var app = builder.Build();

app.UseMiddleware<ErroMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Petzin API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));
app.MapGet("/", () => Results.Redirect("/swagger"));

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
