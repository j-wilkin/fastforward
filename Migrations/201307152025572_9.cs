namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
           // AlterColumn("dbo.Questions", "Result", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
           // AlterColumn("dbo.Questions", "Result", c => c.Int());
        }
    }
}
