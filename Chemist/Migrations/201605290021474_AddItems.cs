namespace Chemist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Medicaments", "Basket_Id", "dbo.Baskets");
            DropForeignKey("dbo.Medicaments", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Medicaments", new[] { "Basket_Id" });
            DropIndex("dbo.Medicaments", new[] { "Order_Id" });
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        Medicament_Id = c.Int(),
                        Basket_Id = c.Int(),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicaments", t => t.Medicament_Id)
                .ForeignKey("dbo.Baskets", t => t.Basket_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Medicament_Id)
                .Index(t => t.Basket_Id)
                .Index(t => t.Order_Id);
            
            DropColumn("dbo.Medicaments", "Basket_Id");
            DropColumn("dbo.Medicaments", "Order_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Medicaments", "Order_Id", c => c.Int());
            AddColumn("dbo.Medicaments", "Basket_Id", c => c.Int());
            DropForeignKey("dbo.Items", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Items", "Basket_Id", "dbo.Baskets");
            DropForeignKey("dbo.Items", "Medicament_Id", "dbo.Medicaments");
            DropIndex("dbo.Items", new[] { "Order_Id" });
            DropIndex("dbo.Items", new[] { "Basket_Id" });
            DropIndex("dbo.Items", new[] { "Medicament_Id" });
            DropTable("dbo.Items");
            CreateIndex("dbo.Medicaments", "Order_Id");
            CreateIndex("dbo.Medicaments", "Basket_Id");
            AddForeignKey("dbo.Medicaments", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Medicaments", "Basket_Id", "dbo.Baskets", "Id");
        }
    }
}
