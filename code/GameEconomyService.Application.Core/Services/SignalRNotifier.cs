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

        public async Task NotifyBalanceUpdateAsync(string userId, WalletState newState)
        {
            // Отправляем сообщение конкретному пользователю
            // Техдолг: обсудать - userId здесь должен совпадать с тем, что используется в SignalR Context.UserIdentifier
            await _hubContext.Clients.User(userId).SendAsync(SignalRConfigContract.PushBalanceUpdated, newState);
        }
    }
}
