using GameEconomyService.Contracts.Models;

namespace GameEconomyService.Contracts
{
    // Контракт: Интерфейс Сервиса игровой экономики - то, что поставляем пользователям Unity пакета
    public interface IGameEconomyService
    {
        /// <summary>
        /// Выполняет подключение к сокету, аутентификацию по DeviceID
        /// и загрузку начального состояния. Устанавливает флаг IsInitialized в true в случае успеха.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Флаг, который становится true только после успешного завершения InitializeAsync() 
        /// и получения первого актуального стейта с сервера.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Событие срабатывает, когда баланс изменился (после покупки или начисления валют админом).
        /// Потребитель (UI) должен подписаться сюда и перерисовать цифры.
        /// </summary>
        event Action<List<CurrencyBalance>> OnBalanceUpdated;


        /// <summary>
        /// Событие срабатывает, гейм-дизайнер обновляет конфигурацию валют.
        /// </summary>
        event Action<EconomyConfig> OnConfigUpdated;

        /// <summary>
        /// Возвращает текущий баланс валют игрока из локальной памяти.
        /// </summary>
        List<CurrencyBalance> GetCachedBalances();

        /// <summary>
        /// Возвращает список всех валют.
        /// </summary>
        List<Currency> GetCachedCurrencies();

        /// <summary>
        /// Попытка обменять одну валюту на другую по актуальному курсу.
        /// Отправляет команду через сокет.
        /// </summary>
        /// <param name="sourceCurrencyId">ID валюты, которую планируем потратить.</param>
        /// <param name="targetCurrencyId">ID валюты, которую хотим получить.</param>
        /// <param name="unitsToBuy">Сколько единиц валюты TargetCurrency хотим получить (согласно курсу).</param>
        Task<ExchangeResult> ExchangeCurrencyAsync(string sourceCurrencyId, string targetCurrencyId, long unitsToBuy);
    }
}

