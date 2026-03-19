namespace PhishingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWebsiteModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebsiteModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        SentEmails = c.Int(nullable: false),
                        FormsFilled = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebsiteModels");
        }
    }
}
