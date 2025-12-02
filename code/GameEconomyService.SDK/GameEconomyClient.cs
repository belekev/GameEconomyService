using GameEconomyService.Contracts;
using GameEconomyService.Contracts.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace GameEconomyService.SDK
{
    /// <summary>
    /// Минимальная реализация сервиса для дальнейшей проработки архитектуры.
    /// </summary>
    public class GameEconomyClient : IGameEconomyService
    {
        // Техдолг: Хардкод для простоты, далее надо рефакторить работу над получением URL бекенда и DeviceId клиента. 
        private const string HUB_URL = "http://localhost:5000/hub/economy";
        private const string HARDCODED_DEVICE_ID = "TEST-HARDCODED-DEVICE-ID-12345";

        private HubConnection _connection;
        private bool _isInitialized = false;

        // Кэш балансов игрока
        private List<CurrencyBalance> _cachedBalances = new List<CurrencyBalance>();
        // Кэш конфигурации валют
        private List<Currency> _cachedCurrencies = new List<Currency>();

        // Объект для синхронизации доступа к кэшу 
        private readonly object _stateLock = new object();

        // Свойства
        public bool IsInitialized => _isInitialized;

        // События
        public event Action<List<CurrencyBalance>> OnBalanceUpdated;
        public event Action<EconomyConfig> OnConfigUpdated;

        // Инициализация

        /// <summary>
        /// Выполняет подключение к сокету и загрузку начального состояния.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_isInitialized) return;

            // Соединение
            _connection = new HubConnectionBuilder()
                .WithUrl(HUB_URL, options =>
                {
                    // Используем захардкоженный DeviceID для аутентификации
                    options.AccessTokenProvider = () => Task.FromResult(HARDCODED_DEVICE_ID);
                })
                .WithAutomaticReconnect()
                .Build();

            // Регистрация хендлеров обновления от сервера
            RegisterServerHandlers();

            // Подключение к серверу
            await _connection.StartAsync();

            // Загрузка начального состояния
            var initialState = await _connection.InvokeAsync<(EconomyConfig, WalletState)>(SignalRConfigContract.RpcLoadInitialState);

            // Сохранение начального состояния в кэш
            UpdateConfigCache(initialState.Item1);
            UpdateBalanceCache(initialState.Item2);

            _isInitialized = true;
        }

        // Методы чтения мз кэша
        public List<CurrencyBalance> GetCachedBalances()
        {
            lock (_stateLock)
            {
                if (!_isInitialized)
                    throw new InvalidOperationException("SDK is not initialized. Call InitializeAsync() first.");

                return _cachedBalances.ToList();
            }
        }

        /// <summary>
        /// Возвращает текущую конфигурацию валют из локального кэша.
        /// </summary>
        public List<Currency> GetCachedCurrencies()
        {
            lock (_stateLock)
            {
                if (!_isInitialized)
                    throw new InvalidOperationException("SDK is not initialized. Call InitializeAsync() first.");

                // Возвращаем копию списка для безопасности
                return _cachedCurrencies.ToList();
            }
        }

        // Метод покупки(обмена) валюты
        public async Task<ExchangeResult> ExchangeCurrencyAsync(string sourceCurrencyId, string targetCurrencyId, long unitsToBuy)
        {
            if (!_isInitialized)
            {
                // Обмдумать при обработке ошибок
                return new ExchangeResult { IsSuccess = false, ErrorCode = "SDK_NOT_INITIALIZED" };
            }

            // Вызов метода на сервере
            var result = await _connection.InvokeAsync<ExchangeResult>(
                SignalRConfigContract.RpcExchangeCurrency,
                sourceCurrencyId,
                targetCurrencyId,
                unitsToBuy
            );

            return result;
        }

        private void RegisterServerHandlers()
        {
            // Обработчик обновления баланса (PUSH)
            _connection.On<WalletState>(SignalRConfigContract.PushBalanceUpdated, walletState =>
            {
                UpdateBalanceCache(walletState);
            });

            // Обработчик обновления конфигурации (PUSH)
            _connection.On<EconomyConfig>(SignalRConfigContract.PushConfigUpdated, config =>
            {
                UpdateConfigCache(config);
            });
        }

        private void UpdateBalanceCache(WalletState walletState)
        {
            lock (_stateLock)
            {
                _cachedBalances = walletState.Balances.ToList();
            }
            OnBalanceUpdated?.Invoke(_cachedBalances);
        }

        private void UpdateConfigCache(EconomyConfig config)
        {
            lock (_stateLock)
            {
                _cachedCurrencies = config.Currencies.ToList();
            }
            OnConfigUpdated?.Invoke(config);
        }
    }
}