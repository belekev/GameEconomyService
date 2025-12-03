namespace GameEconomyService.Contracts
{
    /// <summary>
    /// Константы кодов ошибок для взаимодействия клиент-серверного взаимодействия
    /// Клиент (Unity) может использовать некоторые их для локализации и отображения в UI
    /// </summary>
    public static class EconomyErrorCodes
    {
        // Ошибки валидации
        public const string InvalidAmount = "INVALID_AMOUNT";
        public const string InvalidCurrency = "INVALID_CURRENCY";

        // Ошибки бизнес-логики
        public const string InsufficientFunds = "INSUFFICIENT_FUNDS";
        public const string CurrencyNotFound = "CURRENCY_NOT_FOUND";
        public const string ExchangeRateNotFound = "EXCHANGE_RATE_NOT_FOUND";
        public const string PlayerNotFound = "PLAYER_NOT_FOUND";

        // Системные ошибки
        public const string InternalError = "INTERNAL_ERROR";
    }
}