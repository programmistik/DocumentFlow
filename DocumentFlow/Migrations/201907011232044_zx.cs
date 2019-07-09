namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WhoEdited = c.String(),
                        EditionData = c.DateTime(nullable: false),
                        ClassName = c.String(),
                        ObjectId = c.Int(nullable: false),
                        ObjectJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Histories");
        }
    }
}
