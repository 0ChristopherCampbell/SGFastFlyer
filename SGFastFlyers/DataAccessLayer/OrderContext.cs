using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using SGFastFlyers.Models;

namespace SGFastFlyers.DataAccessLayer
{
    public class OrderContext : DbContext
    {
        public OrderContext() : base("OrderContext")
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public DbSet<PrintDetail> PrintDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

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
        }
    }
}