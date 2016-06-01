namespace Chemist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCab : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CurentSession", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CurentSession");
        }
    }
}
