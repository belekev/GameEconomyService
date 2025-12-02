using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Application.Core.Models;
using GameEconomyService.Contracts.Models;
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
            var currency = new Currency
            {
                Id = request.Id,
                DisplayNameCode = request.DisplayNameCode,
                ExchangeRates = new List<CurrencyExchangeRate>()
            };

            await _repository.AddOrUpdateCurrencyAsync(currency);

            // После изменения данных сразу уведомляем всех клиентов
            await NotifyClientsAsync();
        }

        public async Task SetExchangeRateAsync(UpdateRateRequest request)
        {
            var rate = new CurrencyExchangeRate
            {
                TargetCurrencyId = request.TargetCurrencyId,
                SourceCurrencyAmount = request.SourceAmount,
                TargetCurrencyAmount = request.TargetAmount
            };

            await _repository.UpdateExchangeRateAsync(request.SourceCurrencyId, rate);

            await NotifyClientsAsync();
        }

        public async Task<EconomyConfig> GetCurrentConfigAsync()
        {
            return await _repository.GetConfigAsync();
        }

        private async Task NotifyClientsAsync()
        {
            var config = await _repository.GetConfigAsync();
            await _notifier.NotifyConfigUpdateAsync(config);
        }
    }
}
