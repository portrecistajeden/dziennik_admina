namespace dziennik_Admina.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Journals", "NAME_JOURNAL", c => c.String(nullable: false));
            AlterColumn("dbo.Roles", "NAME", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "NAME", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Journals", "NAME_JOURNAL", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
