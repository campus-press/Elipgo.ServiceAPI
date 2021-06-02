using System;
using System.Collections.Generic;
using System.Text;
using Examen.Elipgo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Examen.Elipgo.DAL.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Article

            builder.Entity<Article>()
                .Property(x => x.Name).HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<Article>()
                .Property(x => x.Description).HasColumnName("description")
                .HasMaxLength(100);

            builder.Entity<Article>()
                .Property(x => x.Code).HasColumnName("code")
                .HasMaxLength(30);

            builder.Entity<Article>()
                .Property(x => x.Price).HasColumnName("price")
                .IsRequired();

            builder.Entity<Article>()
                .Property(x => x.Quantity).HasColumnName("quantity")
                .IsRequired();

            builder.Entity<Article>()
                .Property(x => x.TotalInShelf).HasColumnName("total_in_shelf")
                .IsRequired();

            builder.Entity<Article>()
                .Property(x => x.TotalInVault).HasColumnName("total_in_vault")
                .IsRequired();

            builder.Entity<Article>()
                .Property(x => x.StoreId).HasColumnName("store_id")
                .IsRequired();

            builder.Entity<Article>()
                .HasKey(x => x.Id);

            builder.Entity<Article>()
                .Property(x => x.Id)
                .UseIdentityColumn();

            builder.Entity<Article>()
                .HasIndex(x => x.Code)
                .IsUnique();

            builder.Entity<Article>(a =>
            {
                a.Property(x => x.Price).HasColumnType("decimal(18,2)");
            });

            builder.Entity<Article>()
                .HasOne<Store>(s => s.Store)
                .WithMany(g => g.Articles)
                .HasForeignKey(s => s.StoreId);

            #endregion

            #region Store

            builder.Entity<Store>()
                .Property(x => x.Name).HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<Store>()
                .Property(x => x.Address).HasColumnName("address")
                .HasMaxLength(60)
                .IsRequired();

            builder.Entity<Store>()
                .HasKey(x => x.Id);

            builder.Entity<Store>()
                .Property(x => x.Id)
                .UseIdentityColumn();

            #endregion
        }
        
        public DbSet<Store> Stores { get; set; }
        public DbSet<Article> Articles { get; set; }

        
    }
}
