using GameEconomyService.Contracts.Models;

namespace GameEconomyService.Application.Core.Interfaces
{
    public interface IGameEconomyNotifier
    {
        /// <summary>
        /// Отправляет всем подключенным клиентам обновленную конфигурацию
        /// </summary>
        Task NotifyConfigUpdateAsync(EconomyConfig newConfig);
    }
}
