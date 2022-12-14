using Microsoft.EntityFrameworkCore;
using Movie.Models;

namespace Movie.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Actor> Actor { get; set; }
        public DbSet<Cast> Cast { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<Country> Country { get; set; } 
        public DbSet<Director> Director { get; set; }
        public DbSet<Flim> Flim { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Studio> Studio { get; set; }

        public DbSet<User> User { get; set; }
    }
}
