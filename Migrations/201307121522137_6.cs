namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Careers", "Summary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Careers", "Summary");
        }
    }
}
