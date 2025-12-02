using System.ComponentModel.DataAnnotations;

namespace GameEconomyService.Application.Core.Models
{
    /// <summary>
    /// Запрос на создание новой валюты
    /// Используется в админке
    /// </summary>
    public class CreateCurrencyRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DisplayNameCode { get; set; }
    }
}
