using System;
using AutoLotDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AutoLotDALCore.EF
{
    public class AutoLotContext : DbContext
    {
        public DbSet<CreditRisk> CreditRisks { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Inventory> Cars { get; set; }

        public DbSet<Order> Orders { get; set; }

        public AutoLotContext(DbContextOptions options) : base(options) { }

        internal AutoLotContext() { }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionStrings.ConnectionStr,
                    options => options.EnableRetryOnFailure())
                    .ConfigureWarnings(warn => warn.Throw(RelationalEventId
                    .QueryPossibleUnintendedUseOfEqualsWarning));
                // EventId only for fun because it's about log this event
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create index with several columns
            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasIndex(e => new { e.FirstName, e.LastName })
                .IsUnique();
            });

            // Set cascade parameter in relation
            modelBuilder.Entity<Order>()
                .HasOne(e => e.Car)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        public string GetTableName(Type type)
        {
            return Model.FindEntityType(type).GetTableName();
        }
    }

    public static class ConnectionStrings
    {
        public const string ConnectionStr = @"server=(localdb)\MSSQLLocalDB;" +
            @"database=AutoLotCore;integrated security=True;" +
            @"MultipleActiveResultSets=True;App=EntityFramework;";
    }
}
