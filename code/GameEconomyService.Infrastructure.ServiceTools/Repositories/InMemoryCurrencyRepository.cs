using GameEconomyService.Contracts.Models;
using GameEconomyService.Domain.Interfaces;

namespace GameEconomyService.Infrastructure.ServiceTools.Repositories
{
    /// <summary>
    /// Простая реализация репозитория валют в памяти для тестования
    /// </summary>
    public class InMemoryCurrencyRepository : ICurrencyRepository
    {
        // Храним в памяти статический список для имитации БД
        private static readonly List<Currency> _currencies = new List<Currency>();

        public Task<EconomyConfig> GetConfigAsync()
        {
            var config = new EconomyConfig
            {
                Currencies = _currencies.ToList()
            };
            return Task.FromResult(config);
        }

        public Task AddOrUpdateCurrencyAsync(Currency currency)
        {
            var existing = _currencies.FirstOrDefault(c => c.Id == currency.Id);
            if (existing != null)
            {
                _currencies.Remove(existing);
            }
            _currencies.Add(currency);
            return Task.CompletedTask;
        }

        public async Task UpdateExchangeRateAsync(int sourceId, CurrencyExchangeRate rate)
        {
            var currency = _currencies.FirstOrDefault(c => c.Id == sourceId);
            if (currency != null)
            {
                // Удаляем старый курс к этой целевой валюте, если есть
                currency.ExchangeRates.RemoveAll(r => r.TargetCurrencyId == rate.TargetCurrencyId);
                currency.ExchangeRates.Add(rate);
            }
            await Task.CompletedTask;
        }
    }
}
