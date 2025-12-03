namespace GameEconomyService.Domain.Models
{
    /// <summary>
    /// Валюта и список её курсов обмена
    /// Доменная модель, используемая только внутри слоя Domain
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Id валюты
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Уникальный код валюты, соответствующий DisplayNameCode из контракта.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Список доступных курсов обмена этой валюты на другие
        /// </summary>
        public List<CurrencyExchangeRate> ExchangeRates { get; set; } = new List<CurrencyExchangeRate>();
    }
}
