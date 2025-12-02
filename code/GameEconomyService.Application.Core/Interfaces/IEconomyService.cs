using GameEconomyService.Contracts.Models;

namespace GameEconomyService.Application.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для игровых клиентов
    /// Взаимодействие поставляемого компонента с хабом экономики по сокетам
    /// </summary>
    public interface IEconomyService
    {
        Task<(EconomyConfig Config, WalletState State)> LoadInitialStateAsync(string userId);
        Task<ExchangeResult> ExchangeCurrencyAsync(string userId, string sourceCurrencyId, string targetCurrencyId, long unitsToBuy);
    }
}
