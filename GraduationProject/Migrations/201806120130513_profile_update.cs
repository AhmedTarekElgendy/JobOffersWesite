namespace GraduationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profile_update : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Profiles", new[] { "user_Id" });
            DropColumn("dbo.Profiles", "UserId");
            RenameColumn(table: "dbo.Profiles", name: "user_Id", newName: "UserId");
            AlterColumn("dbo.Profiles", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Profiles", "Birth", c => c.DateTime());
            AlterColumn("dbo.Profiles", "Experience", c => c.Int());
            CreateIndex("dbo.Profiles", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Profiles", new[] { "UserId" });
            AlterColumn("dbo.Profiles", "Experience", c => c.Int(nullable: false));
            AlterColumn("dbo.Profiles", "Birth", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Profiles", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Profiles", name: "UserId", newName: "user_Id");
            AddColumn("dbo.Profiles", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Profiles", "user_Id");
        }
    }
}
