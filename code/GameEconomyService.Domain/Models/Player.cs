using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEconomyService.Domain.Models
{
    /// <summary>
    /// Доменная модель игрока, хранящая его кошелек (балансы).
    /// Отвечает за инварианты баланса (всегда >= 0, операции дебета/кредита).
    /// </summary>
    public class Player
    {
        public int UserId { get; private set; } // Уникальный ID игрока (ключ)

        /// <summary>
        /// Кошелек: ключ - DisplayNameCode валюты, значение - Amount (в наименьших единицах).
        /// </summary>
        public Dictionary<int, long> Wallet { get; private set; } = new Dictionary<int, long>();

        // Приватный конструктор для EF Core / Сериализаторов
        private Player() { }

        public Player(int userId, Dictionary<int, long>? initialWallet = null)
        {
            UserId = userId;
            Wallet = initialWallet ?? new Dictionary<int, long>();
        }

        /// <summary>
        /// Проверяет, достаточно ли средств у игрока для совершения операции.
        /// </summary>
        public bool HasEnoughFunds(int currencyId, long amount)
        {
            if (amount <= 0) return true; // Всегда достаточно, если не тратим ничего

            return Wallet.TryGetValue(currencyId, out long currentBalance) && currentBalance >= amount;
        }

        /// <summary>
        /// Списывает средства с кошелька игрока.
        /// </summary>
        public void Debit(int currencyId, long amount)
        {
            if (amount <= 0) return;
            if (!HasEnoughFunds(currencyId, amount))
            {
                throw new InvalidOperationException($"Недостаточно средств {currencyId}. Требуется {amount}.");
            }

            Wallet[currencyId] -= amount;
        }

        /// <summary>
        /// Зачисляет средства на кошелек игрока.
        /// </summary>
        public void Credit(int currencyId, long amount)
        {
            if (amount <= 0) return;

            if (Wallet.ContainsKey(currencyId))
            {
                // Защита от переполнения long (технический долг, но лучше упомянуть)
                if (Wallet[currencyId] > long.MaxValue - amount)
                {
                    throw new OverflowException($"Попытка переполнения баланса {currencyId}.");
                }
                Wallet[currencyId] += amount;
            }
            else
            {
                Wallet.Add(currencyId, amount);
            }
        }
    }
}
