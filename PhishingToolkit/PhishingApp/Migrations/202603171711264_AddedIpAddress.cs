namespace PhishingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIpAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VictimModels", "IpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VictimModels", "IpAddress");
        }
    }
}
