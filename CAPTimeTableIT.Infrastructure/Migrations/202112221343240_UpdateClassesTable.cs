namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClassesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "AmountSVDK", c => c.Int());
            AddColumn("dbo.Classes", "Blank", c => c.Int());
            AddColumn("dbo.Classes", "CapacityToContain", c => c.Int());
            AddColumn("dbo.Classes", "Credits", c => c.Int());
            AddColumn("dbo.Classes", "NumberOfTKB", c => c.Int());
            AddColumn("dbo.Classes", "PH", c => c.String());
            AddColumn("dbo.Classes", "PH_X", c => c.String());
            AddColumn("dbo.Classes", "SchoolWeek2", c => c.String());
            AddColumn("dbo.Classes", "SoTietDaXep", c => c.Int());
            AddColumn("dbo.Classes", "StatusLHP", c => c.String());
            AddColumn("dbo.Classes", "ThuS", c => c.Int());
            AddColumn("dbo.Classes", "TietS", c => c.Int());
            AddColumn("dbo.Classes", "TMSH", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "TMSH");
            DropColumn("dbo.Classes", "TietS");
            DropColumn("dbo.Classes", "ThuS");
            DropColumn("dbo.Classes", "StatusLHP");
            DropColumn("dbo.Classes", "SoTietDaXep");
            DropColumn("dbo.Classes", "SchoolWeek2");
            DropColumn("dbo.Classes", "PH_X");
            DropColumn("dbo.Classes", "PH");
            DropColumn("dbo.Classes", "NumberOfTKB");
            DropColumn("dbo.Classes", "Credits");
            DropColumn("dbo.Classes", "CapacityToContain");
            DropColumn("dbo.Classes", "Blank");
            DropColumn("dbo.Classes", "AmountSVDK");
        }
    }
}
