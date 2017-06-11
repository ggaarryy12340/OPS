namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PDCategories",
                c => new
                    {
                        PDCategoryId = c.Int(nullable: false, identity: true),
                        PDCategoryName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PDCategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.String(nullable: false, maxLength: 128),
                        PDCategoryId = c.Int(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 60),
                        RegularPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageURL = c.String(),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
            DropTable("dbo.PDCategories");
        }
    }
}
