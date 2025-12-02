using GameEconomyService.Application.Core.Hubs;
using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Application.Core.Services;
using GameEconomyService.Domain.Interfaces;
using GameEconomyService.Infrastructure.ServiceTools.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация слоев, Composition Root
builder.Services.AddSingleton<ICurrencyRepository, InMemoryCurrencyRepository>(); // Временное решение - Singleton, чтобы данные жили в памяти
builder.Services.AddTransient<IEconomyService, EconomyService>();
builder.Services.AddTransient<IEconomyManagementService, EconomyManagementService>();
builder.Services.AddTransient<IGameEconomyNotifier, SignalRNotifier>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<EconomyHub>("/hub/economy");

app.Run();
