using GameEconomyService.Application.Core.Models;
using GameEconomyService.Contracts.Models;

namespace GameEconomyService.Application.Core.Interfaces
{
    /// <summary>
    /// Интерфейс для управления экономикой со стороны админки
    /// </summary>
    public interface IEconomyManagementService
    {
        Task CreateCurrencyAsync(CreateCurrencyRequest request);
        Task SetExchangeRateAsync(UpdateRateRequest request);
        Task<EconomyConfig> GetCurrentConfigAsync();
    }
}
