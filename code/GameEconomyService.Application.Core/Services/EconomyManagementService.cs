using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Application.Core.Models;
using GameEconomyService.Domain.Interfaces;

namespace GameEconomyService.Application.Core.Services
{
    /// <summary>
    /// Реализация сервиса для управления экономикой со стороны админки
    /// </summary>
    public class EconomyManagementService : IEconomyManagementService
    {
        private readonly ICurrencyRepository _repository;
        private readonly IGameEconomyNotifier _notifier;

        public EconomyManagementService(ICurrencyRepository repository, IGameEconomyNotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        public async Task CreateCurrencyAsync(CreateCurrencyRequest request)
        {
            var currency = new Domain.Models.Currency
            {
                Code = request.DisplayNameCode,
                ExchangeRates = new List<Domain.Models.CurrencyExchangeRate>()
            };

            await _repository.AddOrUpdateCurrencyAsync(currency);

            // После изменения данных сразу уведомляем всех клиентов
            await NotifyClientsAsync();
        }

        public async Task SetExchangeRateAsync(UpdateRateRequest request)
        {
            var rate = new Domain.Models.CurrencyExchangeRate(request.TargetCurrencyId, request.SourceAmount, request.TargetAmount);

            await _repository.UpdateExchangeRateAsync(request.SourceCurrencyId, rate);

            await NotifyClientsAsync();
        }

        public async Task<Domain.Models.EconomyConfig> GetCurrentConfigAsync()
        {
            return await _repository.GetConfigAsync();
        }

        private async Task NotifyClientsAsync()
        {
            // Получаем доменную модель
            var domainConfig = await _repository.GetConfigAsync();

            // Маппим в DTO для отправки через SignalR
            var contractsConfig = MapToContractsConfig(domainConfig);

            await _notifier.NotifyConfigUpdateAsync(contractsConfig);
        }

        /// <summary>
        /// Выполняет маппинг из доменной модели в DTO для контрактов.
        /// </summary>
        private Contracts.Models.EconomyConfig MapToContractsConfig(Domain.Models.EconomyConfig domainConfig)
        {
            var contractsCurrencies = domainConfig.Currencies.Select(dCurrency =>
            {
                var contractsRates = dCurrency.ExchangeRates.Select(dRate => new Contracts.Models.CurrencyExchangeRate
                {
                    TargetCurrencyId = dRate.TargetCurrencyId,
                    SourceCurrencyAmount = dRate.SourceCurrencyAmount, // SourceAmount в домене = PriceAmount в контракте
                    TargetCurrencyAmount = dRate.TargetCurrencyAmount  // TargetAmount в домене = RewardAmount в контракте
                }).ToList();

                return new Contracts.Models.Currency
                {
                    Id = dCurrency.Id,
                    DisplayNameCode = dCurrency.Code, // Используем Code как заглушку для Name
                    ExchangeRates = contractsRates
                };
            }).ToList();

            return new Contracts.Models.EconomyConfig
            {
                Currencies = contractsCurrencies
            };
        }
    }
}
