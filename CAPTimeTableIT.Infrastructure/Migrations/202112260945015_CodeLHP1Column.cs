namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeLHP1Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "CodeLHP1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "CodeLHP1");
        }
    }
}
