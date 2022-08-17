namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMaNganhToStringType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classes", "MaNganh", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Classes", "MaNganh", c => c.Int());
        }
    }
}
