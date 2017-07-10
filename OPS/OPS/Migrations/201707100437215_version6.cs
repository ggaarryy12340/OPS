namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PickingId", c => c.Guid());
            AlterColumn("dbo.Orders", "Weight", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.Orders", "PickingId");
            AddForeignKey("dbo.Orders", "PickingId", "dbo.Pickings", "PickingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "PickingId", "dbo.Pickings");
            DropIndex("dbo.Orders", new[] { "PickingId" });
            AlterColumn("dbo.Orders", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "PickingId");
        }
    }
}
