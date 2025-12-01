namespace GameEconomyService.Contracts.Models
{
    /// <summary>
    /// Валюта и курсы её обмена.
    /// </summary>
    [Serializable]
    public class Currency
    {
        // Уникальный Id валюты
        public int Id { get; set; }
        // Код, который является ключем для отображения/локализации имени в Unity
        public string DisplayNameCode { get; set; }
        // Список доступных курсов обмена этой валюты на другие
        public List<CurrencyExchangeRate> ExchangeRates { get; set; } = new List<CurrencyExchangeRate>();
    }
}
