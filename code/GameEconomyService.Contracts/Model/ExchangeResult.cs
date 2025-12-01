namespace GameEconomyService.Contracts.Models
{
    /// <summary>
    /// Результат попытки обмена валюты.
    /// </summary>
    [Serializable]
    public class ExchangeResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
    }
}