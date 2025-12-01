namespace GameEconomyService.Contracts.Models
{
    /// <summary>
    /// Информация о курсе обмена валют.
    /// </summary>
    [Serializable]
    public struct CurrencyExchangeRate
    {
        // Id валюты, за которую покупаем другую валюту
        public int TargetCurrencyId;
        // Сколько платим в исходной валюте
        public long SourceCurrencyAmount;
        // Сколько получаем приобретаемой валюты
        public long TargetCurrencyAmount;      
    }
}
