namespace GameEconomyService.Domain.Models
{
    /// <summary>
    /// Доменная модель для конфигурации экономики это по сути список валют с курсами
    /// </summary>
    public class EconomyConfig
    {
        public List<Currency> Currencies { get; private set; } = new List<Currency>();

        public EconomyConfig(List<Currency> currencies)
        {
            Currencies = currencies ?? new List<Currency>();
        }

        public EconomyConfig() { }

        // Методы для валидации?
    }
}
