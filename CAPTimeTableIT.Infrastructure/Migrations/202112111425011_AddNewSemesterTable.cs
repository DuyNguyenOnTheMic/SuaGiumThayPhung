/*namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddNewSemesterTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Semesters",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EndYear = c.Int(nullable: false),
                    Name = c.String(),
                    CreatedDateTime = c.DateTime(nullable: false),
                    LastModifiedDateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Semesters");
        }
    }
}
*/