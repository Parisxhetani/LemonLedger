using GameService.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace GameService.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        // <-- add these:
        public DbSet<Player> Players { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<SavingsAccount> SavingsAccounts { get; set; }
    }
}
