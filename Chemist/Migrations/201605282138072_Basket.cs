namespace Chemist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Basket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Baskets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Medicaments", "Basket_Id", c => c.Int());
            CreateIndex("dbo.Medicaments", "Basket_Id");
            AddForeignKey("dbo.Medicaments", "Basket_Id", "dbo.Baskets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Medicaments", "Basket_Id", "dbo.Baskets");
            DropIndex("dbo.Medicaments", new[] { "Basket_Id" });
            DropColumn("dbo.Medicaments", "Basket_Id");
            DropTable("dbo.Baskets");
        }
    }
}
