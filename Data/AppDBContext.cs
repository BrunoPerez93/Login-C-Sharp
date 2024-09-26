using LoginApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(table =>
            {
                table.HasKey(col => col.IdUser);
                table.Property(col => col.IdUser)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                table.Property(col => col.Name).HasMaxLength(50);
                table.Property(col => col.Email).HasMaxLength(50);
                table.Property(col => col.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Car>(table =>
            {
                table.HasKey(col => col.IdCar);
                table.Property(col => col.IdCar)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                table.Property(col => col.Year).HasMaxLength(4);
            });

            modelBuilder.Entity<Car>().ToTable("Cars");
        }
    }
}
