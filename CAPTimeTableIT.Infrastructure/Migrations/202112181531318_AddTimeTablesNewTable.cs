namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeTablesNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeTableCode = c.String(),
                        TimeTableFile = c.String(),
                        CreateDate = c.DateTime(),
                        ClassId = c.Int(),
                        SemesterId = c.Int(),
                        UserID = c.Int(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        LastModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TimeTables");
        }
    }
}
