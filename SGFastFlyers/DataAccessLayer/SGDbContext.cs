using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using SGFastFlyers.Models;

namespace SGFastFlyers.DataAccessLayer
{
    public class SGDbContext : DbContext
    {
        public SGDbContext() : base("SGDbContext")
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public DbSet<PrintDetail> PrintDetails { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Model Relationship Definitions
            modelBuilder.Entity<PrintDetail>()
                .HasKey(t => t.OrderID);

            modelBuilder.Entity<DeliveryDetail>()
                .HasKey(t => t.OrderID);

            modelBuilder.Entity<Order>()
                .HasRequired(t => t.PrintDetail)
                .WithRequiredPrincipal(t => t.Order);

            modelBuilder.Entity<Order>()
                .HasRequired(t => t.DeliveryDetail)
                .WithRequiredPrincipal(t => t.Order);

            modelBuilder.Entity<Order>()
                .HasOptional(t => t.Quote)
                .WithOptionalPrincipal(t => t.Order);
            #endregion
        }
    }
}