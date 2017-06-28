namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "PickingId", "dbo.Pickings");
            DropIndex("dbo.Orders", new[] { "PickingId" });
            RenameColumn(table: "dbo.Orders", name: "PickingId", newName: "Picking_PickingId");
            AlterColumn("dbo.Orders", "Picking_PickingId", c => c.Guid());
            CreateIndex("dbo.Orders", "Picking_PickingId");
            AddForeignKey("dbo.Orders", "Picking_PickingId", "dbo.Pickings", "PickingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Picking_PickingId", "dbo.Pickings");
            DropIndex("dbo.Orders", new[] { "Picking_PickingId" });
            AlterColumn("dbo.Orders", "Picking_PickingId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Orders", name: "Picking_PickingId", newName: "PickingId");
            CreateIndex("dbo.Orders", "PickingId");
            AddForeignKey("dbo.Orders", "PickingId", "dbo.Pickings", "PickingId", cascadeDelete: true);
        }
    }
}
