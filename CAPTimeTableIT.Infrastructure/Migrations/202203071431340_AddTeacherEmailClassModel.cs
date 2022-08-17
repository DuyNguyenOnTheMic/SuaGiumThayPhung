namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherEmailClassModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "TeacherEmail", c => c.String());
            DropColumn("dbo.Classes", "TeacherName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "TeacherName", c => c.String());
            DropColumn("dbo.Classes", "TeacherEmail");
        }
    }
}
