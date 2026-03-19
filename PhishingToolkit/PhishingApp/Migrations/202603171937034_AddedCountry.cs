namespace PhishingApp.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedCountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VictimModels", "Country", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.VictimModels", "Country");
        }
    }
}
