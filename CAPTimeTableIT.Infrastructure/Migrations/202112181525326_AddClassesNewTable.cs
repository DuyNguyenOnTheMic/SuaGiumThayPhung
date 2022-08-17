namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassesNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        ClassCode = c.String(),
                        ClassType = c.String(),
                        CodeLHP = c.String(),
                        Lesson = c.String(),
                        PeriodStart = c.Int(),
                        PeriodTotal = c.Int(),
                        RoomAdress = c.String(),
                        SemesterNumber = c.Int(),
                        StatusClass = c.Boolean(),
                        TheDayOfTheWeek = c.String(),
                        WeekEnd = c.Int(),
                        WeekStart = c.Int(),
                        BranchOfLearningCode = c.Int(nullable: false),
                        SemesterId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        AspNetUserId = c.Int(nullable: false),
                        BranchOfLearningName = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        LastModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Classes");
        }
    }
}
