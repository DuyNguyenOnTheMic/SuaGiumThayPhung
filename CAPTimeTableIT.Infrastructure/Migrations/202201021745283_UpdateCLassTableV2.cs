namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCLassTableV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "MaGocLHP", c => c.String());
            AddColumn("dbo.Classes", "MaLHP", c => c.String());
            AddColumn("dbo.Classes", "SoTC", c => c.Int());
            AddColumn("dbo.Classes", "LoaiHP", c => c.String());
            AddColumn("dbo.Classes", "MaLop", c => c.String());
            AddColumn("dbo.Classes", "Thu", c => c.String());
            AddColumn("dbo.Classes", "TietBD", c => c.Int());
            AddColumn("dbo.Classes", "SoTiet", c => c.Int());
            AddColumn("dbo.Classes", "TietHoc", c => c.String());
            AddColumn("dbo.Classes", "Phong", c => c.String());
            AddColumn("dbo.Classes", "SucChua", c => c.Int());
            AddColumn("dbo.Classes", "SiSoTKB", c => c.Int());
            AddColumn("dbo.Classes", "TinhTrangLHP", c => c.String());
            AddColumn("dbo.Classes", "TuanHoc2", c => c.String());
            AddColumn("dbo.Classes", "SoSVDK", c => c.Int());
            AddColumn("dbo.Classes", "TuanBD", c => c.Int());
            AddColumn("dbo.Classes", "TuanKT", c => c.Int());
            AddColumn("dbo.Classes", "MaNganh", c => c.Int());
            AddColumn("dbo.Classes", "TenNganh", c => c.String());
            AddColumn("dbo.Classes", "Note", c => c.String());
            AddColumn("dbo.Classes", "UserID", c => c.Int());
            DropColumn("dbo.Classes", "ClassId");
            DropColumn("dbo.Classes", "BranchLearningCode");
            DropColumn("dbo.Classes", "BranchLearningName");
            DropColumn("dbo.Classes", "ClassCode");
            DropColumn("dbo.Classes", "ClassType");
            DropColumn("dbo.Classes", "CodeLHP");
            DropColumn("dbo.Classes", "Lesson");
            DropColumn("dbo.Classes", "PeriodStart");
            DropColumn("dbo.Classes", "PeriodTotal");
            DropColumn("dbo.Classes", "RoomAdress");
            DropColumn("dbo.Classes", "SemesterNumber");
            DropColumn("dbo.Classes", "StatusClass");
            DropColumn("dbo.Classes", "TheDaysOfTheWeek");
            DropColumn("dbo.Classes", "WeekEnd");
            DropColumn("dbo.Classes", "WeekStart");
            DropColumn("dbo.Classes", "CapacityToContain");
            DropColumn("dbo.Classes", "Credits");
            DropColumn("dbo.Classes", "NumberOfTKB");
            DropColumn("dbo.Classes", "SchoolWeek2");
            DropColumn("dbo.Classes", "StatusLHP");
            DropColumn("dbo.Classes", "CodeLHP1");
            DropColumn("dbo.Classes", "AmountSVDK");
            DropColumn("dbo.Classes", "AspNetUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "AspNetUserId", c => c.Int());
            AddColumn("dbo.Classes", "AmountSVDK", c => c.Int());
            AddColumn("dbo.Classes", "CodeLHP1", c => c.String());
            AddColumn("dbo.Classes", "StatusLHP", c => c.String());
            AddColumn("dbo.Classes", "SchoolWeek2", c => c.String());
            AddColumn("dbo.Classes", "NumberOfTKB", c => c.Int());
            AddColumn("dbo.Classes", "Credits", c => c.Int());
            AddColumn("dbo.Classes", "CapacityToContain", c => c.Int());
            AddColumn("dbo.Classes", "WeekStart", c => c.Int());
            AddColumn("dbo.Classes", "WeekEnd", c => c.Int());
            AddColumn("dbo.Classes", "TheDaysOfTheWeek", c => c.String());
            AddColumn("dbo.Classes", "StatusClass", c => c.Boolean());
            AddColumn("dbo.Classes", "SemesterNumber", c => c.Int());
            AddColumn("dbo.Classes", "RoomAdress", c => c.String());
            AddColumn("dbo.Classes", "PeriodTotal", c => c.Int());
            AddColumn("dbo.Classes", "PeriodStart", c => c.Int());
            AddColumn("dbo.Classes", "Lesson", c => c.String());
            AddColumn("dbo.Classes", "CodeLHP", c => c.String());
            AddColumn("dbo.Classes", "ClassType", c => c.String());
            AddColumn("dbo.Classes", "ClassCode", c => c.String());
            AddColumn("dbo.Classes", "BranchLearningName", c => c.String());
            AddColumn("dbo.Classes", "BranchLearningCode", c => c.Int());
            AddColumn("dbo.Classes", "ClassId", c => c.Int(nullable: false));
            DropColumn("dbo.Classes", "UserID");
            DropColumn("dbo.Classes", "Note");
            DropColumn("dbo.Classes", "TenNganh");
            DropColumn("dbo.Classes", "MaNganh");
            DropColumn("dbo.Classes", "TuanKT");
            DropColumn("dbo.Classes", "TuanBD");
            DropColumn("dbo.Classes", "SoSVDK");
            DropColumn("dbo.Classes", "TuanHoc2");
            DropColumn("dbo.Classes", "TinhTrangLHP");
            DropColumn("dbo.Classes", "SiSoTKB");
            DropColumn("dbo.Classes", "SucChua");
            DropColumn("dbo.Classes", "Phong");
            DropColumn("dbo.Classes", "TietHoc");
            DropColumn("dbo.Classes", "SoTiet");
            DropColumn("dbo.Classes", "TietBD");
            DropColumn("dbo.Classes", "Thu");
            DropColumn("dbo.Classes", "MaLop");
            DropColumn("dbo.Classes", "LoaiHP");
            DropColumn("dbo.Classes", "SoTC");
            DropColumn("dbo.Classes", "MaLHP");
            DropColumn("dbo.Classes", "MaGocLHP");
        }
    }
}
