using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Contracts.Models;
using GameEconomyService.Domain.Interfaces;

namespace GameEconomyService.Application.Core.Services
{
    /// <summary>
    /// Реализация сервиса для игровых клиентов.
    /// </summary>
    public class EconomyService : IEconomyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public EconomyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<(EconomyConfig Config, WalletState State)> LoadInitialStateAsync(string userId)
        {
            // Загрузка актуальной конфигурации экономики
            var config = await _currencyRepository.GetConfigAsync();

            // Техдолг: Загружаем кошелек игрока (ПОКА ЗАГЛУШКА)
            // Возвращаем пустой кошелек пока
            var emptyWallet = new WalletState();

            return (config, emptyWallet);
        }

        public Task<ExchangeResult> ExchangeCurrencyAsync(string userId, string sourceCurrencyId, string targetCurrencyId, long unitsToBuy)
        {
            // Техдолг: Пока не реализовано
            throw new NotImplementedException();
        }
    }
}
