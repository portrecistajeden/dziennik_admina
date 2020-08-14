namespace dziennik_Admina.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        ID_JOURNAL = c.Int(nullable: false, identity: true),
                        NAME_JOURNAL = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID_JOURNAL);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NAME = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users_Roles",
                c => new
                    {
                        ID_USER = c.Int(nullable: false),
                        ID_ROLE = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_USER, t.ID_ROLE })
                .ForeignKey("dbo.Roles", t => t.ID_ROLE, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ID_USER, cascadeDelete: true)
                .Index(t => t.ID_USER)
                .Index(t => t.ID_ROLE);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        USERNAME = c.String(nullable: false, maxLength: 20),
                        PASWORD = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users_Roles", "ID_USER", "dbo.Users");
            DropForeignKey("dbo.Users_Roles", "ID_ROLE", "dbo.Roles");
            DropIndex("dbo.Users_Roles", new[] { "ID_ROLE" });
            DropIndex("dbo.Users_Roles", new[] { "ID_USER" });
            DropTable("dbo.Users");
            DropTable("dbo.Users_Roles");
            DropTable("dbo.Roles");
            DropTable("dbo.Journals");
        }
    }
}
