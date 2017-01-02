namespace SGFastFlyers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmallChangePrintDetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrintDetail", "PrintSize", c => c.Int());
            AlterColumn("dbo.PrintDetail", "PrintFormat", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PrintDetail", "PrintFormat", c => c.Int(nullable: false));
            AlterColumn("dbo.PrintDetail", "PrintSize", c => c.Int(nullable: false));
        }
    }
}
