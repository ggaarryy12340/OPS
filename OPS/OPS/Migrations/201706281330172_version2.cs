namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ConvenienceStoreName", c => c.String(maxLength: 20));
            AddColumn("dbo.Orders", "ConvenienceStoreNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.Orders", "DeliveryWay", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Orders", "Distributor", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Distributor", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.Orders", "DeliveryWay", c => c.String(nullable: false, maxLength: 2));
            DropColumn("dbo.Orders", "ConvenienceStoreNo");
            DropColumn("dbo.Orders", "ConvenienceStoreName");
        }
    }
}
