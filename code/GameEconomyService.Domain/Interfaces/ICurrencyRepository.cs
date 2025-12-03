using GameEconomyService.Domain.Models;

namespace GameEconomyService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с конфигурацией экономики (валюты и курсы)
    /// Использует только доменные модели.
    /// </summary>
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Возвращает полную конфигурацию экономики (все валюты и их курсы обмена)
        /// </summary>
        Task<EconomyConfig> GetConfigAsync();

        /// <summary>
        /// Добавляет/обновляет валюту
        /// </summary>
        Task AddOrUpdateCurrencyAsync(Currency currency);

        /// <summary>
        /// Обновляет курс обмена для указанной валюты
        /// </summary>
        Task UpdateExchangeRateAsync(int currencyId, CurrencyExchangeRate rate);

        /// <summary>
        /// Удаляет валюту
        /// </summary>
        Task DeleteCurrencyAsync(int Id);
    }
}