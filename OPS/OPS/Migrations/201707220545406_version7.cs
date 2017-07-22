namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PickingMakeLogs", "DeliveryWay", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PickingMakeLogs", "DeliveryWay", c => c.String(maxLength: 2));
        }
    }
}
