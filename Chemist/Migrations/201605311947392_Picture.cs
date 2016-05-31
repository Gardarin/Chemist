namespace Chemist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Picture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medicaments", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.Medicaments", "Count");
        }
    }
}
