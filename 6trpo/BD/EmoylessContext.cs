using _6trpo.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace _6trpo.BD
{
   public class EmoylessContext:DbContext

    {
        public EmoylessContext(DbContextOptions<EmoylessContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;");
        }
    }
}
