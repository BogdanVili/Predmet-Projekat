namespace PhishingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedNameEmailsSent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Websites", "EmailsSent", c => c.Int(nullable: false));
            DropColumn("dbo.Websites", "SentEmails");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Websites", "SentEmails", c => c.Int(nullable: false));
            DropColumn("dbo.Websites", "EmailsSent");
        }
    }
}
