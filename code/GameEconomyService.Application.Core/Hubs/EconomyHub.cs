using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Contracts.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameEconomyService.Application.Core.Hubs
{
    // Техдолг: надо подумать над аттирбутами типа [Authorize]
    public class EconomyHub : Hub
    {
        private readonly IEconomyService _economyService;

        public EconomyHub(IEconomyService economyService)
        {
            _economyService = economyService;
        }

        // Клиент вызывает этот метод при старте
        public async Task<(EconomyConfig Config, WalletState State)> LoadInitialState()
        {
            // Техдолг: Пока используем временный ID или из Context
            string userId = Context.UserIdentifier ?? "anonymous_user";

            return await _economyService.LoadInitialStateAsync(userId);
        }

        // Техдолг: Тут должен быть метод обмена
    }
}
