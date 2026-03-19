namespace PhishingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VictimModels", "Timestamp", c => c.Long(nullable: false));
            DropColumn("dbo.VictimModels", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VictimModels", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.VictimModels", "Timestamp");
        }
    }
}
