namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClassModelLV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "TeacherId", c => c.String());
            AddColumn("dbo.Classes", "CreatedAssignerId", c => c.String());
            AddColumn("dbo.Classes", "LastModifiedAssignerId", c => c.String());
            DropColumn("dbo.Classes", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "UserID", c => c.Int());
            DropColumn("dbo.Classes", "LastModifiedAssignerId");
            DropColumn("dbo.Classes", "CreatedAssignerId");
            DropColumn("dbo.Classes", "TeacherId");
        }
    }
}
