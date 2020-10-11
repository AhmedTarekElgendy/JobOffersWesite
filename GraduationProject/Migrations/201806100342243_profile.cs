namespace GraduationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        img = c.String(),
                        Name = c.String(),
                        Birth = c.DateTime(nullable: false),
                        Sex = c.String(),
                        State = c.String(),
                        Military = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        PostalZip = c.String(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.user_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profiles", "user_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Profiles", new[] { "user_Id" });
            DropTable("dbo.Profiles");
        }
    }
}
