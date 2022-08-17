namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcontraint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Semesters", "ListWeek", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Semesters", "ListWeek", c => c.String());
        }
    }
}
