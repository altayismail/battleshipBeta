using battleshipBeta.Entities;
using Microsoft.EntityFrameworkCore;

namespace battleshipBeta.Database
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=ISMAIL; database=BattleshipDB; integrated security=true; MultipleActiveResultSets=true;");
        }

        public DbSet<ExcelObjectAI>? excelObjectAIs { get; set; }
        public DbSet<ExcelObjectTuttorial>? excelObjectTuttorials { get; set; }
        
    }
}
