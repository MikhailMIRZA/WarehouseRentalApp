using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TestWorkindForEducation.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавить Сервис в контейрнер
builder.Services.AddControllers();

// Добавить DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped<IStorageUnitRepository, StorageUnitRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Регистрация Сервисов
builder.Services.AddScoped<IStorageUnitService, StorageUnitService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddHttpClient(); // Для HttpClient
builder.Services.AddScoped<StorageApiService>(); // Для нашего сервиса

// Добавить Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();

var app = builder.Build();

// Принимает миграции автоматически
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); 
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.UseStaticFiles();
app.MapBlazorHub();
app.MapFallbackToPage("/Shared/_Host");
app.Run();