namespace GameEconomyService.Domain.Models
{
    /// <summary>
    /// Курс обмена: описывает, сколько покупаемой дается за одну единицу валюты, которую тратим
    /// Это чистая доменная модель, используемая только внутри слоя Domain
    /// </summary>
    public class CurrencyExchangeRate
    {
        // Id валюты, которую приобретаем
        public int TargetCurrencyId { get; set; }
        // Сколько платим в исходной валюте
        public long SourceCurrencyAmount { get; private set; }
        // Сколько получаем приобретаемой валюты
        public long TargetCurrencyAmount { get; private set; }

        // Приватный конструктор для EF Core
        private CurrencyExchangeRate() { }

        // Основной конструктор с валидацией доменного уровня
        public CurrencyExchangeRate(int targetCurrencyId, long sourceAmount, long targetAmount)
        {
            if (sourceAmount <= 0 || targetAmount <= 0)
                throw new ArgumentException("Exchange amounts must be positive.");

            TargetCurrencyId = targetCurrencyId;
            SourceCurrencyAmount = sourceAmount;
            TargetCurrencyAmount = targetAmount;
        }
    }
}
