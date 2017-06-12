namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Guid(nullable: false),
                        SourceOrderId = c.String(nullable: false, maxLength: 50),
                        OrderDateTime = c.DateTime(),
                        OrderStatus = c.String(nullable: false, maxLength: 2),
                        DeliveryWay = c.String(nullable: false, maxLength: 2),
                        Distributor = c.String(nullable: false, maxLength: 2),
                        RecieveName = c.String(nullable: false, maxLength: 10),
                        RecievePhone = c.String(nullable: false, maxLength: 12),
                        RecieveZipCode = c.String(nullable: false, maxLength: 5),
                        RecieveAddress = c.String(nullable: false),
                        OrderPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Feight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Payment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrackingNo = c.String(maxLength: 50),
                        PickingId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Pickings", t => t.PickingId, cascadeDelete: true)
                .Index(t => t.PickingId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.Guid(nullable: false),
                        ProductName = c.String(maxLength: 100),
                        Spec = c.String(maxLength: 20),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderId = c.Guid(nullable: false),
                        ProductId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(nullable: false, maxLength: 60),
                        RegularPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageURL = c.String(),
                        PDCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.PDCategories", t => t.PDCategoryId, cascadeDelete: true)
                .Index(t => t.PDCategoryId);
            
            CreateTable(
                "dbo.PDCategories",
                c => new
                    {
                        PDCategoryId = c.Int(nullable: false),
                        PDCategoryName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PDCategoryId);
            
            CreateTable(
                "dbo.PickingMakeLogs",
                c => new
                    {
                        PickingMakeLogId = c.Guid(nullable: false),
                        PickingMakeLogTime = c.DateTime(),
                        RoundQty = c.Int(nullable: false),
                        DeliveryWay = c.String(maxLength: 2),
                        Order_OrderId = c.Guid(),
                    })
                .PrimaryKey(t => t.PickingMakeLogId)
                .ForeignKey("dbo.Orders", t => t.Order_OrderId)
                .Index(t => t.Order_OrderId);
            
            CreateTable(
                "dbo.Pickings",
                c => new
                    {
                        PickingId = c.Guid(nullable: false),
                        PickingDateTime = c.DateTime(),
                        Round = c.String(nullable: false, maxLength: 10),
                        IsComplete = c.String(nullable: false, maxLength: 2),
                        PickingMakeLogId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PickingId)
                .ForeignKey("dbo.PickingMakeLogs", t => t.PickingMakeLogId, cascadeDelete: true)
                .Index(t => t.PickingMakeLogId);
            
            CreateTable(
                "dbo.OrderStatusLogs",
                c => new
                    {
                        OrderStatusLogId = c.Guid(nullable: false),
                        OrderCreateTime = c.DateTime(),
                        OrderPickingTime = c.DateTime(),
                        OrderPackingTime = c.DateTime(),
                        OrderAlreadyTime = c.DateTime(),
                        OrderDeliveryTime = c.DateTime(),
                        OrderId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OrderStatusLogId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderStatusLogs", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "PickingId", "dbo.Pickings");
            DropForeignKey("dbo.PickingMakeLogs", "Order_OrderId", "dbo.Orders");
            DropForeignKey("dbo.Pickings", "PickingMakeLogId", "dbo.PickingMakeLogs");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "PDCategoryId", "dbo.PDCategories");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderStatusLogs", new[] { "OrderId" });
            DropIndex("dbo.Pickings", new[] { "PickingMakeLogId" });
            DropIndex("dbo.PickingMakeLogs", new[] { "Order_OrderId" });
            DropIndex("dbo.Products", new[] { "PDCategoryId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "PickingId" });
            DropTable("dbo.OrderStatusLogs");
            DropTable("dbo.Pickings");
            DropTable("dbo.PickingMakeLogs");
            DropTable("dbo.PDCategories");
            DropTable("dbo.Products");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
        }
    }
}
