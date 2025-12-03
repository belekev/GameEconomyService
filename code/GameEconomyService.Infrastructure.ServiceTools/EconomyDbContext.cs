using GameEconomyService.Infrastructure.ServiceTools.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameEconomyService.Infrastructure.ServiceTools
{
    /// <summary>
    /// Контекст базы данных Entity Framework Core для работы с PostgreSQL
    /// </summary>
    public class EconomyDbContext : DbContext
    {
        public EconomyDbContext(DbContextOptions<EconomyDbContext> options)
            : base(options)
        {
        }

        public DbSet<CurrencyEntity> Currencies { get; set; }
        // public DbSet<WalletEntity> Wallets { get; set; }
        // Подумать насчет Player

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CurrencyEntity>(entity =>
            {
                entity.ToTable("Currencies");

                // Добавляем уникальный индекс для поля Code, чтоб в БД не было двух валют с одинаковым кодом
                entity.HasIndex(e => e.Code)
                      .IsUnique();
            });
        }
    }
}