namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SemesterAndCapWeekAddFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CapWeeks", "Semester_Id", c => c.Int());
            CreateIndex("dbo.CapWeeks", "Semester_Id");
            AddForeignKey("dbo.CapWeeks", "Semester_Id", "dbo.Semesters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CapWeeks", "Semester_Id", "dbo.Semesters");
            DropIndex("dbo.CapWeeks", new[] { "Semester_Id" });
            DropColumn("dbo.CapWeeks", "Semester_Id");
        }
    }
}
