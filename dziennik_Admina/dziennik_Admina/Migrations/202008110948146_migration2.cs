namespace dziennik_Admina.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "USERNAME", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "PASWORD", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "PASWORD", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "USERNAME", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
