using DotNetCrudWebApi.Books;
using Microsoft.EntityFrameworkCore;

namespace DotNetCrudWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BookModel> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./data/App.db");
        }
    }
}
