namespace SGFastFlyers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeliveryDetail",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        DeliveryArea = c.String(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Order", t => t.OrderID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PrintDetail",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        NeedsPrint = c.Boolean(nullable: false),
                        PrintSize = c.Int(nullable: false),
                        PrintFormat = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Order", t => t.OrderID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Quote",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsMetro = c.Boolean(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Order", t => t.OrderID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quote", "OrderID", "dbo.Order");
            DropForeignKey("dbo.PrintDetail", "OrderID", "dbo.Order");
            DropForeignKey("dbo.DeliveryDetail", "OrderID", "dbo.Order");
            DropIndex("dbo.Quote", new[] { "OrderID" });
            DropIndex("dbo.PrintDetail", new[] { "OrderID" });
            DropIndex("dbo.DeliveryDetail", new[] { "OrderID" });
            DropTable("dbo.Quote");
            DropTable("dbo.PrintDetail");
            DropTable("dbo.Order");
            DropTable("dbo.DeliveryDetail");
        }
    }
}
