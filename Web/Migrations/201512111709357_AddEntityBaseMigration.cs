namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntityBaseMigration : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Films");
            AddColumn("dbo.Films", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Films", "Id");
            DropColumn("dbo.Films", "IdGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Films", "IdGuid", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.Films");
            DropColumn("dbo.Films", "Id");
            AddPrimaryKey("dbo.Films", "IdGuid");
        }
    }
}
