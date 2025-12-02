using GameEconomyService.Contracts.Models;

namespace GameEconomyService.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Возвращает полную конфигурацию экономики (все валюты и курсы обмена)
        /// </summary>
        Task<EconomyConfig> GetConfigAsync();

        /// <summary>
        /// Добавляет или обновляет валюту
        /// </summary>
        Task AddOrUpdateCurrencyAsync(Currency currency);

        /// <summary>
        /// Обновляет курс обмена
        /// </summary>
        Task UpdateExchangeRateAsync(int sourceId, CurrencyExchangeRate rate);

    }
}
