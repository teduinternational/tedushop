namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CustomerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.ApplicationUsers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropColumn("dbo.Orders", "CustomerId");
        }
    }
}
