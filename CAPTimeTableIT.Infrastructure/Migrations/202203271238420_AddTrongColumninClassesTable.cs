namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrongColumninClassesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "Trong", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "Trong");
        }
    }
}
