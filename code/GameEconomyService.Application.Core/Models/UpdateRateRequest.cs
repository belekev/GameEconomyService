namespace GameEconomyService.Application.Core.Models
{
    /// <summary>
    /// Модель для обновления курса обмена валют
    /// </summary>
    public class UpdateRateRequest
    {
        public int SourceCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        public long SourceAmount { get; set; }
        public long TargetAmount { get; set; }
    }
}
