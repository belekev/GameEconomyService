using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Domain.Interfaces;
using GameEconomyService.Contracts;

namespace GameEconomyService.Application.Core.Services
{
    /// <summary>
    /// Реализация сервиса для игровых клиентов.
    /// </summary>
    public class EconomyService : IEconomyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameEconomyNotifier _notifier;

        public EconomyService(
            ICurrencyRepository currencyRepository,
            IPlayerRepository playerRepository,
            IGameEconomyNotifier notifier)
        {
            _currencyRepository = currencyRepository;
            _playerRepository = playerRepository;
            _notifier = notifier;
        }

        /// <summary>
        /// Загружает начальную конфигурацию и состояние кошелька игрока
        /// </summary>
        public async Task<(Contracts.Models.EconomyConfig Config, Contracts.Models.WalletState State)> LoadInitialStateAsync(string userId)
        {
            // Загрузка доменной конфигурации и игрока
            var domainConfig = await _currencyRepository.GetConfigAsync();
            var player = await _playerRepository.GetPlayerAsync(userId);

            // Если игрока нет, создаем нового
            if (player == null)
            {
                player = await _playerRepository.CreatePlayerAsync(userId);
            }

            // Маппинг в DTO
            var contractsConfig = MapToContractsConfig(domainConfig);
            var walletState = MapToContractsWalletState(player);

            return (contractsConfig, walletState);
        }

        /// <summary>
        /// Выполняет обмен валют по их ID
        /// </summary>
        /// <param name="sourceCurrencyId">ID валюты, которую отдаем (покупаем).</param>
        /// <param name="targetCurrencyId">ID валюты, которую получаем (продаем).</param>
        public async Task<Contracts.Models.ExchangeResult> ExchangeCurrencyAsync(string userId, int sourceCurrencyId, int targetCurrencyId, long unitsToBuy)
        {
            // Валидация входных данных, пока просто задел на будущее
            if (unitsToBuy <= 0)
            {
                return new Contracts.Models.ExchangeResult { IsSuccess = false, ErrorCode = EconomyErrorCodes.InvalidAmount };
            }

            // Получение данных игрока и конфигурации
            var player = await _playerRepository.GetPlayerAsync(userId);
            if (player == null)
            {
                return new Contracts.Models.ExchangeResult { IsSuccess = false, ErrorCode = EconomyErrorCodes.PlayerNotFound };
            }

            var domainConfig = await _currencyRepository.GetConfigAsync();

            // Поиск валют и курса по ID
            // Ищем доменную Currency по ID
            var sourceCurrencyDomain = domainConfig.Currencies.FirstOrDefault(c => c.Id == sourceCurrencyId);

            if (sourceCurrencyDomain == null)
            {
                return new Contracts.Models.ExchangeResult { IsSuccess = false, ErrorCode = EconomyErrorCodes.CurrencyNotFound };
            }

            // Ищем курс по TargetCurrencyId
            var rate = sourceCurrencyDomain.ExchangeRates
                .FirstOrDefault(r => r.TargetCurrencyId == targetCurrencyId);

            if (rate == null)
            {
                return new Contracts.Models.ExchangeResult { IsSuccess = false, ErrorCode = EconomyErrorCodes.ExchangeRateNotFound };
            }

            // Расчет стоимости
            // Используем доменные имена SourceCurrencyAmount и TargetCurrencyAmount
            long totalCost = rate.SourceCurrencyAmount * unitsToBuy;
            long totalReward = rate.TargetCurrencyAmount * unitsToBuy;

            // Проверка средств
            if (!player.HasEnoughFunds(sourceCurrencyId, totalCost))
            {
                return new Contracts.Models.ExchangeResult { IsSuccess = false, ErrorCode = EconomyErrorCodes.InsufficientFunds };
            }

            // Выполнение транзакции
            try
            {
                player.Debit(sourceCurrencyId, totalCost);
                player.Credit(targetCurrencyId, totalReward);

                await _playerRepository.UpdatePlayerAsync(player);
            }
            catch (Exception)
            {
                return new Contracts.Models.ExchangeResult { IsSuccess = false, ErrorCode = EconomyErrorCodes.InternalError };
            }

            // Формирование нового состояния и уведомление
            var walletDto = MapToContractsWalletState(player);

            // Отправляем PUSH уведомление
            await _notifier.NotifyBalanceUpdateAsync(userId, walletDto);

            return new Contracts.Models.ExchangeResult { IsSuccess = true };
        }

        // --- Методы маппинга ---

        private Contracts.Models.EconomyConfig MapToContractsConfig(Domain.Models.EconomyConfig domainConfig)
        {
            var contractsCurrencies = domainConfig.Currencies.Select(dCurrency =>
            {
                var contractsRates = dCurrency.ExchangeRates.Select(dRate => new Contracts.Models.CurrencyExchangeRate
                {
                    SourceCurrencyAmount = dRate.SourceCurrencyAmount,
                    TargetCurrencyAmount = dRate.TargetCurrencyAmount,
                    TargetCurrencyId = dRate.TargetCurrencyId
                }).ToList();

                return new Contracts.Models.Currency
                {
                    Id = dCurrency.Id,
                    DisplayNameCode = dCurrency.Code,
                    ExchangeRates = contractsRates
                };
            }).ToList();

            return new Contracts.Models.EconomyConfig
            {
                Currencies = contractsCurrencies
            };
        }

        private Contracts.Models.WalletState MapToContractsWalletState(Domain.Models.Player player)
        {
            var newBalances = player.Wallet.Select(x => new Contracts.Models.CurrencyBalance
            {
                CurrencyId = x.Key,
                Amount = x.Value
            }).ToList();

            return new Contracts.Models.WalletState { Balances = newBalances };
        }
    }
}