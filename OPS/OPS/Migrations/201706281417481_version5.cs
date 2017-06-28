namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Picking_PickingId", "dbo.Pickings");
            DropIndex("dbo.Orders", new[] { "Picking_PickingId" });
            DropColumn("dbo.Orders", "Picking_PickingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Picking_PickingId", c => c.Guid());
            CreateIndex("dbo.Orders", "Picking_PickingId");
            AddForeignKey("dbo.Orders", "Picking_PickingId", "dbo.Pickings", "PickingId");
        }
    }
}
