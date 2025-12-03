using GameEconomyService.Domain.Models;

namespace GameEconomyService.Domain.Interfaces
{
    /// <summary>
    /// Репозиторий для доменной модели Player
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// Получает доменную модель игрока по его ID
        /// </summary>
        Task<Player?> GetPlayerAsync(string userId);

        /// <summary>
        /// Обновляет состояние игрока (напр., после изменения баланса)
        /// </summary>
        Task UpdatePlayerAsync(Player player);

        /// <summary>
        /// Создает нового игрока с начальным состоянием
        /// </summary>
        Task<Player> CreatePlayerAsync(string userId);
    }
}
