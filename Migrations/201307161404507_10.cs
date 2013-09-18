namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsDivider", c => c.Boolean(nullable: false));
            DropColumn("dbo.Events", "UrlLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "UrlLink", c => c.String());
            DropColumn("dbo.Events", "IsDivider");
        }
    }
}
