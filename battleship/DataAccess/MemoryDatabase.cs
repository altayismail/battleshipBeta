using battleship.Models;
using Microsoft.EntityFrameworkCore;

namespace battleship.DataAccess
{
    public class MemoryDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BattleshipDB");
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Computer>? Computers { get; set; }
    }
}
