using GameEconomyService.Contracts.Models;

namespace GameEconomyService.Application.Core.Interfaces
{
    public interface IGameEconomyNotifier
    {
        /// <summary>
        /// Отправляет всем подключенным клиентам обновленную конфигурацию
        /// </summary>
        Task NotifyConfigUpdateAsync(EconomyConfig newConfig);


        /// <summary>
        /// Отправляет обновленное состояние кошелька
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        Task NotifyBalanceUpdateAsync(string userId, WalletState newState);
    }
}
