namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCapWeeksNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CapWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        LastModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CapWeeks");
        }
    }
}
