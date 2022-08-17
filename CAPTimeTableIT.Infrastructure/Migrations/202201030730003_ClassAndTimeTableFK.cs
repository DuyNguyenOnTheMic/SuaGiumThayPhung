namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassAndTimeTableFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "TimeTables_Id", c => c.Int());
            CreateIndex("dbo.Classes", "TimeTables_Id");
            AddForeignKey("dbo.Classes", "TimeTables_Id", "dbo.TimeTables", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Classes", "TimeTables_Id", "dbo.TimeTables");
            DropIndex("dbo.Classes", new[] { "TimeTables_Id" });
            DropColumn("dbo.Classes", "TimeTables_Id");
        }
    }
}
