namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PickingMakeLogs", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.PickingMakeLogs", new[] { "Order_OrderId" });
            DropColumn("dbo.PickingMakeLogs", "Order_OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PickingMakeLogs", "Order_OrderId", c => c.Guid());
            CreateIndex("dbo.PickingMakeLogs", "Order_OrderId");
            AddForeignKey("dbo.PickingMakeLogs", "Order_OrderId", "dbo.Orders", "OrderId");
        }
    }
}
