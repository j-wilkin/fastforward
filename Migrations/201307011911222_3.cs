namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocalColleges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        College = c.String(),
                        StateId = c.Int(nullable: false),
                        State = c.String(),
                        Website = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LocalColleges");
        }
    }
}
