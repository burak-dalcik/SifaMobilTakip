using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using sifam.AutoMapper; // AutoMapper MappingProfile'� i�in namespace ekliyoruz

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�
builder.Services.AddDbContext<sifam.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS Ayarlar�
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// AutoMapper hizmetini ekle
builder.Services.AddAutoMapper(typeof(MappingProfile)); // MappingProfile s�n�f�n� AutoMapper'a ekliyoruz

// Controller'lar� ekleyin
builder.Services.AddControllers();

// Swagger Ayarlar�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "�ifa Mobil API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "�ifa Mobil API v1");
    });
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
