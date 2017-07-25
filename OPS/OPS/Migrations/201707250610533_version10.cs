namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PickingProductChecks",
                c => new
                    {
                        pickingProductCheckId = c.Guid(nullable: false),
                        ProductImageURL = c.String(),
                        Quantity = c.Int(),
                        CheckedQuantity = c.Int(),
                        IsChecked = c.String(maxLength: 2),
                        PickingId = c.Guid(nullable: false),
                        ProductId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.pickingProductCheckId)
                .ForeignKey("dbo.Pickings", t => t.PickingId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.PickingId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PickingProductChecks", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PickingProductChecks", "PickingId", "dbo.Pickings");
            DropIndex("dbo.PickingProductChecks", new[] { "ProductId" });
            DropIndex("dbo.PickingProductChecks", new[] { "PickingId" });
            DropTable("dbo.PickingProductChecks");
        }
    }
}
