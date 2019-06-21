namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.Documents", new[] { "OrganizationId" });
            AlterColumn("dbo.Documents", "OrganizationId", c => c.Int());
            AlterColumn("dbo.Documents", "ContactId", c => c.Int());
            CreateIndex("dbo.Documents", "OrganizationId");
            AddForeignKey("dbo.Documents", "OrganizationId", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.Documents", new[] { "OrganizationId" });
            AlterColumn("dbo.Documents", "ContactId", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "OrganizationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "OrganizationId");
            AddForeignKey("dbo.Documents", "OrganizationId", "dbo.Organizations", "Id", cascadeDelete: true);
        }
    }
}
