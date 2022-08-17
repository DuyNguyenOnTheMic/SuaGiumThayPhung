namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CapDayAndCapWeekAddFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CapDays", "CapWeek_Id", c => c.Int());
            CreateIndex("dbo.CapDays", "CapWeek_Id");
            AddForeignKey("dbo.CapDays", "CapWeek_Id", "dbo.CapWeeks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CapDays", "CapWeek_Id", "dbo.CapWeeks");
            DropIndex("dbo.CapDays", new[] { "CapWeek_Id" });
            DropColumn("dbo.CapDays", "CapWeek_Id");
        }
    }
}
