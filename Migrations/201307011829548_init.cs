namespace fastforward.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        SurveyId = c.Int(nullable: false, identity: true),
                        SurveyName = c.String(nullable: false),
                        Headline = c.String(),
                    })
                .PrimaryKey(t => t.SurveyId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false),
                        Label = c.String(),
                        Result = c.Int(nullable: false),
                        IsViewable = c.Boolean(nullable: false),
                        SurveyId = c.Int(nullable: false),
                        GroupIdA = c.Int(nullable: false),
                        GroupIdB = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Checkboxes",
                c => new
                    {
                        CheckboxId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Checked = c.Boolean(nullable: false),
                        Question_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.CheckboxId)
                .ForeignKey("dbo.Questions", t => t.Question_QuestionId)
                .Index(t => t.Question_QuestionId);
            
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        CareerId = c.Int(nullable: false, identity: true),
                        CareerName = c.String(),
                        Salary = c.String(),
                        Education = c.String(),
                        Image = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CareerId)
                .ForeignKey("dbo.CareerGroups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.CareerGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Headline = c.String(),
                        TextContent = c.String(),
                        Comment = c.String(),
                        IsSelfPost = c.Boolean(nullable: false),
                        Index = c.Int(nullable: false),
                        Image = c.String(),
                        UrlLink = c.String(),
                        Major = c.Boolean(nullable: false),
                        CareerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Careers", t => t.CareerId, cascadeDelete: true)
                .Index(t => t.CareerId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Events", new[] { "CareerId" });
            DropIndex("dbo.Careers", new[] { "GroupId" });
            DropIndex("dbo.Checkboxes", new[] { "Question_QuestionId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "SurveyId" });
            DropForeignKey("dbo.Events", "CareerId", "dbo.Careers");
            DropForeignKey("dbo.Careers", "GroupId", "dbo.CareerGroups");
            DropForeignKey("dbo.Checkboxes", "Question_QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "SurveyId", "dbo.Surveys");
            DropTable("dbo.Events");
            DropTable("dbo.CareerGroups");
            DropTable("dbo.Careers");
            DropTable("dbo.Checkboxes");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
            DropTable("dbo.Surveys");
        }
    }
}
