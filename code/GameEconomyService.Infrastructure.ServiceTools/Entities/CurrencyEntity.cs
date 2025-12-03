using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameEconomyService.Infrastructure.ServiceTools.Entities
{
    /// <summary>
    /// Сущность, представляющая валюту в БД
    /// Используется EF Core для маппинга на таблицу Currencies
    /// </summary>
    [Table("Currencies")]
    public class CurrencyEntity
    {
        /// <summary>
        /// Уникальный Id валюты, первичный ключ. Соответствует Id в контракте
        /// </summary>
        [Key]
        // Указываем, что Id генерируется базой данных
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Код валюты
        /// Это ключ DisplayNameCode из контракта. Должен быть уникальным
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
    }
}