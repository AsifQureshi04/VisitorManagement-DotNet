using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace MFL_VisitorManagement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuthentication>()
                    .HasKey(x => x.Id);

            modelBuilder.Entity<UserAuthentication>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserAuthentication>()
                       .Property(e => e.IsActive)
                       .HasConversion(v => v == true ? '1' : '0', v => v == '1'); 
            modelBuilder.Entity<UserAuthentication>()
                       .Property(e => e.IsDelete)
                       .HasConversion(v => v == true ? '1' : '0', v => v == '1');

            modelBuilder.Entity<UserAuthentication>().ToTable("USERAUTHENTICATION");
        }

        public DbSet<UserAuthentication> UserAuthentication { get; set; }  
        public DbSet<UserLoginResponse> UserLoginResponse { get; set; }
        public DbSet<Users> Users { get; set; }


    }
}
