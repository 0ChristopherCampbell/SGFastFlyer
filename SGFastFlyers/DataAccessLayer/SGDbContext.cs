using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using SGFastFlyers.Models;
using SGFastFlyers.Controllers;

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
        public DbSet<AttachmentDetail> AttachmentDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Model Relationship Definitions
                #region Order Detail Relations
                modelBuilder.Entity<PrintDetail>()
                    .HasKey(t => t.OrderID);

            modelBuilder.Entity<AttachmentDetail>()
                   .HasKey(t => t.OrderID);

            modelBuilder.Entity<DeliveryDetail>()
                    .HasKey(t => t.OrderID);

                modelBuilder.Entity<Quote>()
                    .HasKey(t => t.OrderID);

                modelBuilder.Entity<Order>()
                    .HasRequired(t => t.PrintDetail)
                    .WithRequiredPrincipal(t => t.Order);

                modelBuilder.Entity<Order>()
                    .HasRequired(t => t.DeliveryDetail)
                    .WithRequiredPrincipal(t => t.Order);

                modelBuilder.Entity<Order>()
                    .HasRequired(t => t.Quote)
                    .WithRequiredPrincipal(t => t.Order);
                #endregion

                #region Some other relationship Definitions

                #endregion
            #endregion
        }
    }
}