namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RelatedOccupations",
                c => new
                    {
                        RelatedId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CareerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RelatedId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RelatedOccupations");
        }
    }
}
