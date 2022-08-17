namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClassTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classes", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.Classes", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.Classes", new[] { "SemesterId" });
            DropIndex("dbo.Classes", new[] { "SubjectId" });
            AddColumn("dbo.Classes", "BranchLearningCode", c => c.Int());
            AddColumn("dbo.Classes", "BranchLearningName", c => c.String());
            AddColumn("dbo.Classes", "TheDaysOfTheWeek", c => c.String());
            AlterColumn("dbo.Classes", "SemesterId", c => c.Int());
            AlterColumn("dbo.Classes", "SubjectId", c => c.Int());
            AlterColumn("dbo.Classes", "AspNetUserId", c => c.Int());
            CreateIndex("dbo.Classes", "SubjectId");
            CreateIndex("dbo.Classes", "SemesterId");
            AddForeignKey("dbo.Classes", "SemesterId", "dbo.Semesters", "Id");
            AddForeignKey("dbo.Classes", "SubjectId", "dbo.Subjects", "Id");
            DropColumn("dbo.Classes", "TheDayOfTheWeek");
            DropColumn("dbo.Classes", "BranchOfLearningCode");
            DropColumn("dbo.Classes", "BranchOfLearningName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "BranchOfLearningName", c => c.String());
            AddColumn("dbo.Classes", "BranchOfLearningCode", c => c.Int(nullable: false));
            AddColumn("dbo.Classes", "TheDayOfTheWeek", c => c.String());
            DropForeignKey("dbo.Classes", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Classes", "SemesterId", "dbo.Semesters");
            DropIndex("dbo.Classes", new[] { "SemesterId" });
            DropIndex("dbo.Classes", new[] { "SubjectId" });
            AlterColumn("dbo.Classes", "AspNetUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Classes", "SubjectId", c => c.Int(nullable: false));
            AlterColumn("dbo.Classes", "SemesterId", c => c.Int(nullable: false));
            DropColumn("dbo.Classes", "TheDaysOfTheWeek");
            DropColumn("dbo.Classes", "BranchLearningName");
            DropColumn("dbo.Classes", "BranchLearningCode");
            CreateIndex("dbo.Classes", "SubjectId");
            CreateIndex("dbo.Classes", "SemesterId");
            AddForeignKey("dbo.Classes", "SubjectId", "dbo.Subjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Classes", "SemesterId", "dbo.Semesters", "Id", cascadeDelete: true);
        }
    }
}
