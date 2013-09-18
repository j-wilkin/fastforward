namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Careers", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Careers", "Description");
        }
    }
}
