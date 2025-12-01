namespace GameEconomyService.Contracts.Models
{
    /// <summary>
    /// Полный список всех актуальных валют - "конфигурация экономики".
    /// </summary>
    [Serializable]
    public class EconomyConfig
    {
        // Cписок актуальных валют.
        public List<Currency> Currencies { get; set; } = new List<Currency>();
    }
}
