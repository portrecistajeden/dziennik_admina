namespace dziennik_Admina.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration4 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Journals");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        ID_JOURNAL = c.Int(nullable: false, identity: true),
                        NAME_JOURNAL = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID_JOURNAL);
            
        }
    }
}
