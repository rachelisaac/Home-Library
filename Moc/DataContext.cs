using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;



namespace Moc
{
    public class DataContext:DbContext, IContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-7QELS7G;database=home_library;trusted_connection=true;TrustServerCertificate=True");
        }
        //add-migration init
        //update-database 

    }
}
