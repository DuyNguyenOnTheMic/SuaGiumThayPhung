namespace CAPTimeTableIT.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBlankColumnInClassesTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classes", "Trong", c => c.Int());
            DropColumn("dbo.Classes", "Blank");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Classes", "Blank", c => c.Int());
            AlterColumn("dbo.Classes", "Trong", c => c.Int(nullable: false));
        }
    }
}
