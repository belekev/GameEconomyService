using GameEconomyService.Application.Core.Hubs;
using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Application.Core.Services;
using GameEconomyService.Domain.Interfaces;
using GameEconomyService.Infrastructure.ServiceTools;
using GameEconomyService.Infrastructure.ServiceTools.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- PostgreSQL и EF Core ---
var connectionString = builder.Configuration.GetConnectionString("PostgreSql");

if (string.IsNullOrEmpty(connectionString))
{
    // Еслои строки подключения нет, то будем исползовать пока In-Memory репозиторий
    builder.Services.AddSingleton<ICurrencyRepository, InMemoryCurrencyRepository>();

    // Техдолг: Добавить IUserRepository
}
else
{
    builder.Services.AddDbContext<EconomyDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    // Техдолг: Регистрируем заглушку, пока не создадим PostgreSqlCurrencyRepository
    // Все равное пока юзаем In-Memory
    builder.Services.AddSingleton<ICurrencyRepository, InMemoryCurrencyRepository>();


    // Техдолг: Добавить IUserRepository
}
// ---------------------------------------------------------------------------------

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация слоев, Composition Root
builder.Services.AddTransient<IEconomyService, EconomyService>();
builder.Services.AddTransient<IEconomyManagementService, EconomyManagementService>();
builder.Services.AddTransient<IGameEconomyNotifier, SignalRNotifier>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();
app.MapHub<EconomyHub>("/hub/economy");

app.Run();
