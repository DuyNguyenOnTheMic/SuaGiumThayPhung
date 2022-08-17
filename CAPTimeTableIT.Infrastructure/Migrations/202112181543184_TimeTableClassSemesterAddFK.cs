namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeTableClassSemesterAddFK : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Classes", "SemesterId");
            CreateIndex("dbo.Classes", "SubjectId");
            CreateIndex("dbo.TimeTables", "ClassId");
            CreateIndex("dbo.TimeTables", "SemesterId");
            AddForeignKey("dbo.Classes", "SemesterId", "dbo.Semesters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Classes", "SubjectId", "dbo.Subjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TimeTables", "ClassId", "dbo.Classes", "Id");
            AddForeignKey("dbo.TimeTables", "SemesterId", "dbo.Semesters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeTables", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.TimeTables", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Classes", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Classes", "SemesterId", "dbo.Semesters");
            DropIndex("dbo.TimeTables", new[] { "SemesterId" });
            DropIndex("dbo.TimeTables", new[] { "ClassId" });
            DropIndex("dbo.Classes", new[] { "SubjectId" });
            DropIndex("dbo.Classes", new[] { "SemesterId" });
        }
    }
}
