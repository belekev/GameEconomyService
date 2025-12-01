namespace GameEconomyService.Contracts.Models
{
    /// <summary>
    /// Текущее состояние кошелька игрока.
    /// </summary>
    [Serializable]
    public class WalletState
    {
        // Список актуальных балансов игрока. CurrencyBalance - DTO, 
        // который содержит два поля - int CurrencyId и long Amount.
        public List<CurrencyBalance> Balances { get; set; } = new List<CurrencyBalance>();
    }
}
