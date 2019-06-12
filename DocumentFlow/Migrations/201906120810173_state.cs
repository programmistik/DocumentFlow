namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class state : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentTypes", "DocTypeAcc", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentTypes", "DocTypeAcc");
        }
    }
}
