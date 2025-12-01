namespace GameEconomyService.Contracts
{
    /// <summary>
    /// Статический класс, содержащий имена для коммуникации по сокетам.
    /// Является общим контрактом между GameEconomyService.SDK и GameEconomyService.Backend.
    /// </summary>
    public static class SignalRConfigContract
    {
        /// <summary>
        /// Вызов для обмена одной валюты на другую.
        /// Используется в методе GameEconomyClient.ExchangeCurrencyAsync.
        /// </summary>
        public const string RpcExchangeCurrency = "ExchangeCurrency";

        /// <summary>
        /// Вызов для загрузки начальной конфигурации и балансов.
        /// Используется в методе GameEconomyClient.InitializeAsync.
        /// </summary>
        public const string RpcLoadInitialState = "LoadInitialState";

        /// <summary>
        /// Сообщение отправляется сервером при обновлении конфигурации экономики.
        /// На него подписывается GameEconomyClient.
        /// </summary>
        public const string PushConfigUpdated = "ReceiveConfigUpdate";

        /// <summary>
        /// Сообщение отправляется сервером при изменении баланса игрока.
        /// На него подписывается GameEconomyClient.
        /// </summary>
        public const string PushBalanceUpdated = "ReceiveBalanceUpdate";
    }
}