namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeacherEmailClassModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "TeacherName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "TeacherName");
        }
    }
}
