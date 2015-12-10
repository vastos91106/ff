namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToFilmEntityMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Films", "Name");
        }
    }
}
