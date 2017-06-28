namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "RecieveZipCode", c => c.String(maxLength: 5));
            AlterColumn("dbo.Orders", "RecieveAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "RecieveAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "RecieveZipCode", c => c.String(nullable: false, maxLength: 5));
        }
    }
}
