namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaffCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "StaffCode", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "StaffCode");
        }
    }
}
