/*namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTeacherNameClassModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Classes", "TeacherName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "TeacherName", c => c.String());
        }
    }
}
*/