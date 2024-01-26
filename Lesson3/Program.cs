using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Lesson3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new UserContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Users.Add(new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 36,
                    Position = "Manager",
                    DepartmentId = 1,
                    EmployeeId = 101
                });

                context.Users.Add(new User
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Age = 23,
                    Position = "Employee",
                    DepartmentId = 2
                });

                context.SaveChanges();
            }
        }
        public class User
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Position { get; set; }
            public int DepartmentId { get; set; }
            public int EmployeeId { get; set; }
        }

        public class UserContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=UserDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                //1
                modelBuilder.Entity<User>().HasKey(u => new { u.Id, u.EmployeeId });

                //2
                modelBuilder.Entity<User>().Property(u => u.Position).HasMaxLength(50);

                //3
                modelBuilder.Entity<User>().HasCheckConstraint("Age", "Age > 17");

                //4
                modelBuilder.Entity<User>().HasCheckConstraint("Position", "Position = 'manager' OR Position = 'employee'");

                //5
                modelBuilder.Entity<User>().ToTable("NewTableName").HasKey(u => u.Id).HasName("PK_NewTableName");

                //6
                modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired(false);
                modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired();
                modelBuilder.Entity<User>().Property(u => u.Age).IsRequired();
                modelBuilder.Entity<User>().Property(u => u.Position).IsRequired();
                modelBuilder.Entity<User>().Property(u => u.DepartmentId).IsRequired();
                modelBuilder.Entity<User>().Property(u => u.EmployeeId).IsRequired();
            }
        }
    }
}
