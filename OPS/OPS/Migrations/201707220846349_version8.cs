namespace OPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pickings", "Round", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pickings", "Round", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
