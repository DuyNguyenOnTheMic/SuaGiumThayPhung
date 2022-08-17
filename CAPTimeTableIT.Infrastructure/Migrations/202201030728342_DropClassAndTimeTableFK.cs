namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropClassAndTimeTableFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeTables", "ClassId", "dbo.Classes");
            DropIndex("dbo.TimeTables", new[] { "ClassId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.TimeTables", "ClassId");
            AddForeignKey("dbo.TimeTables", "ClassId", "dbo.Classes", "Id");
        }
    }
}
