namespace GameEconomyService.Contracts.Models
{
    /// <summary>
    /// Актуальный баланс игрока по одной валюте.
    /// </summary>
    [Serializable]
    public struct CurrencyBalance
    {
        public int CurrencyId { get; set; }
        public long Amount { get; set; }
    }
}
