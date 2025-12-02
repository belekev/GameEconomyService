using GameEconomyService.Application.Core.Interfaces;
using GameEconomyService.Application.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameEconomyService.Application.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Техдолг: [Authorize(Roles = "Admin")] // добавить авторизацию?
    public class CurrenciesController : ControllerBase
    {
        private readonly IEconomyManagementService _managementService;

        public CurrenciesController(IEconomyManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Возвращаем всю конфигурацию
            var config = await _managementService.GetCurrentConfigAsync();
            return Ok(config);
        }

        /// <summary>
        /// POST: api/currencies
        /// Создает новую валюту.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Core Service добавляет валюту и уведомляет клиентов через SignalR Notifier
            await _managementService.CreateCurrencyAsync(request);
            return Ok(new { message = "Currency created and clients notified" });
        }

        /// <summary>
        /// POST: api/currencies/rates
        /// Устанавливает курс обмена.
        /// </summary>
        [HttpPost("rates")]
        public async Task<IActionResult> SetRate([FromBody] UpdateRateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Core Service обновлеяет курс и уведомляет клиентов через SignalR Notifier
            await _managementService.SetExchangeRateAsync(request);
            return Ok(new { message = "Rate updated and clients notified" });
        }
    }
}
