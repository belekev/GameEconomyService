using GameEconomyService.Application.Core.Hubs;
using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Contracts;
using GameEconomyService.Contracts.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameEconomyService.Application.Core.Services
{
    /// <summary>
    /// Реализация нотификатора для отправки сообщений клиентам
    /// Использует SignalR
    /// </summary>
    public class SignalRNotifier : IGameEconomyNotifier
    {
        private readonly IHubContext<EconomyHub> _hubContext;

        public SignalRNotifier(IHubContext<EconomyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyConfigUpdateAsync(EconomyConfig newConfig)
        {
            // Отправляем сообщение всем подключенным клиентам
            // Используем константу из общего контракта
            await _hubContext.Clients.All.SendAsync(SignalRConfigContract.PushConfigUpdated, newConfig);
        }
    }
}
