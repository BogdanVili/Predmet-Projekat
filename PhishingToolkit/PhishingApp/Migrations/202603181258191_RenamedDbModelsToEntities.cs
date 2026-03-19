namespace PhishingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedDbModelsToEntities : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.VictimModels", newName: "Victims");
            RenameTable(name: "dbo.WebsiteModels", newName: "Websites");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Websites", newName: "WebsiteModels");
            RenameTable(name: "dbo.Victims", newName: "VictimModels");
        }
    }
}
