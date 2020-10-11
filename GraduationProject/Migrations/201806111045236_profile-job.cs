namespace GraduationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profilejob : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.Jobs", "Country", c => c.String());
            AddColumn("dbo.Jobs", "City", c => c.String());
            AddColumn("dbo.Jobs", "AgeFrom", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "AgeTo", c => c.Int(nullable: false));
            AddColumn("dbo.Jobs", "Gender", c => c.String());
            AddColumn("dbo.Jobs", "State", c => c.String());
            AddColumn("dbo.Jobs", "Military", c => c.String());
            AddColumn("dbo.Profiles", "Bio", c => c.String());
            AddColumn("dbo.Profiles", "Mail", c => c.String());
            AddColumn("dbo.Profiles", "OtherLinks", c => c.String());
            AddColumn("dbo.Profiles", "Gender", c => c.String());
            AddColumn("dbo.Profiles", "Education", c => c.String());
            AddColumn("dbo.Profiles", "Title", c => c.String());
            AddColumn("dbo.Profiles", "Experience", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "CV", c => c.String());
            DropColumn("dbo.Profiles", "Sex");
            DropColumn("dbo.Profiles", "PostalZip");
        }
        
        public override void Down()
        {
            
            AddColumn("dbo.Profiles", "PostalZip", c => c.String());
            AddColumn("dbo.Profiles", "Sex", c => c.String());
            DropColumn("dbo.Profiles", "CV");
            DropColumn("dbo.Profiles", "Experience");
            DropColumn("dbo.Profiles", "Title");
            DropColumn("dbo.Profiles", "Education");
            DropColumn("dbo.Profiles", "Gender");
            DropColumn("dbo.Profiles", "OtherLinks");
            DropColumn("dbo.Profiles", "Mail");
            DropColumn("dbo.Profiles", "Bio");
            DropColumn("dbo.Jobs", "Military");
            DropColumn("dbo.Jobs", "State");
            DropColumn("dbo.Jobs", "Gender");
            DropColumn("dbo.Jobs", "AgeTo");
            DropColumn("dbo.Jobs", "AgeFrom");
            DropColumn("dbo.Jobs", "City");
            DropColumn("dbo.Jobs", "Country");
            
        }
    }
}
