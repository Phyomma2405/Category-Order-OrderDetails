using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Collections.Generic;
using ApiProjectPRN231.Models;

namespace ApiProjectPRN231
{
    public class ApplicationDbContext : IdentityDbContext<UserApp>
    {
        public virtual DbSet<EmailToken> EmailTokens { get; set; }
        //-----------//
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        //-----------//
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmailToken>(en =>
            {
                en.Property(token => token.Id).ValueGeneratedOnAdd();
                en.Property(token => token.UserId).IsRequired();
                en.Property(token => token.ExpiredTime).IsRequired();
                en.Property(token => token.Deleted).IsRequired().HasDefaultValue(false);

                en.HasKey(token => token.Id);
                en.HasOne(token => token.User)
                .WithMany().HasForeignKey(token => token.UserId).IsRequired();
            });

            modelBuilder.Entity<Order>(en =>
            {
                en.Property(o => o.Id).ValueGeneratedOnAdd();
                en.Property(o => o.UserId).IsRequired();
                en.Property(o => o.StatusId).IsRequired();

                en.HasKey(o => o.Id);
                en.HasOne(o => o.User)
                .WithMany().HasForeignKey(o => o.UserId).IsRequired();
                en.HasOne(o => o.Status)
                .WithMany().HasForeignKey(o => o.StatusId).IsRequired();

                en.HasMany(o => o.Details)
               .WithOne(d => d.Order).HasForeignKey(o => o.OrderId).IsRequired();
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(od => new { od.OrderId, od.ProductId });

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.Details)
                    .HasForeignKey(od => od.OrderId);

                entity.HasOne(od => od.Product)
                    .WithMany()
                    .HasForeignKey(od => od.ProductId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(os => os.Id).ValueGeneratedOnAdd();
                entity.HasKey(os => os.Id);
            });

            modelBuilder.Entity<Product>(entity =>
            {

                entity.Property(p => p.Id).ValueGeneratedOnAdd();

                entity.HasOne(p => p.Category)
                .WithMany().HasForeignKey(p => p.CategoryId);

                entity.HasMany(p => p.Feedbacks)
                .WithOne(f => f.Product).HasForeignKey(f => f.ProductId);

                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(f => f.Id).ValueGeneratedOnAdd();

                entity.HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks).HasForeignKey(f => f.ProductId);
                entity.HasOne(f => f.User)
                .WithMany().HasForeignKey(f => f.UserId);
                entity.HasKey(f => f.Id);

            });

            //----------------//
            var hasher = new PasswordHasher<UserApp>();
            modelBuilder.Entity<UserApp>().HasData(
                new UserApp
                {
                    Id = "admin",
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    NormalizedUserName = "ADMIN",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "admin"),
                },
                 new UserApp
                 {
                     Id = "user",
                     Email = "user@gmail.com",
                     UserName = "user",
                     NormalizedEmail = "USER@GMAIL.COM",
                     NormalizedUserName = "USER",
                     EmailConfirmed = true,
                     PasswordHash = hasher.HashPassword(null, "user"),
                 }
            );
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "user",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "admin",
                    RoleId = "admin",
                },
                new IdentityUserRole<string>
                {
                    UserId = "admin",
                    RoleId = "user",
                },
                new IdentityUserRole<string>
                {
                    UserId = "user",
                    RoleId = "user",
                }
            );
            //--------------------//
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Computer" },
                new Category { Id = 2, Name = "Phone" },
                new Category { Id = 3, Name = "Headphone" },
                new Category { Id = 4, Name = "Keyboard" },
                new Category { Id = 5, Name = "Mouse" }
            );
            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Pending" },
                new OrderStatus { Id = 2, Name = "Processing" },
                new OrderStatus { Id = 3, Name = "Shipped" },
                new OrderStatus { Id = 4, Name = "Delivered" },
                new OrderStatus { Id = 5, Name = "Cancelled" }
            );
        }
    }
}
