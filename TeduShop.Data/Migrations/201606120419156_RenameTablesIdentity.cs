namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTablesIdentity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.IdentityUserLogins", newName: "ApplicationUserLogins");
            RenameTable(name: "dbo.IdentityUserRoles", newName: "ApplicationUserRoles");
            RenameTable(name: "dbo.IdentityRoles", newName: "ApplicationUserClaims");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ApplicationUserClaims", newName: "IdentityRoles");
            RenameTable(name: "dbo.ApplicationUserRoles", newName: "IdentityUserRoles");
            RenameTable(name: "dbo.ApplicationUserLogins", newName: "IdentityUserLogins");
        }
    }
}
