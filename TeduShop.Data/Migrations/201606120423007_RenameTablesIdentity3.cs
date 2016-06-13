namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTablesIdentity3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserClaims", newName: "ApplicationRoles");
            RenameTable(name: "dbo.IdentityUserClaims", newName: "ApplicationUserClaims");
         
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ApplicationRoles", newName: "ApplicationUserClaims");
            RenameTable(name: "dbo.ApplicationUserClaims", newName: "IdentityUserClaims");
        }
    }
}
