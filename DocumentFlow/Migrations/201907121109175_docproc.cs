namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class docproc : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskProcesses", "Document_Id", "dbo.Documents");
            DropIndex("dbo.TaskProcesses", new[] { "Document_Id" });
            RenameColumn(table: "dbo.TaskProcesses", name: "Document_Id", newName: "DocId");
            AlterColumn("dbo.TaskProcesses", "DocId", c => c.Int(nullable: false));
            CreateIndex("dbo.TaskProcesses", "DocId");
            AddForeignKey("dbo.TaskProcesses", "DocId", "dbo.Documents", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskProcesses", "DocId", "dbo.Documents");
            DropIndex("dbo.TaskProcesses", new[] { "DocId" });
            AlterColumn("dbo.TaskProcesses", "DocId", c => c.Int());
            RenameColumn(table: "dbo.TaskProcesses", name: "DocId", newName: "Document_Id");
            CreateIndex("dbo.TaskProcesses", "Document_Id");
            AddForeignKey("dbo.TaskProcesses", "Document_Id", "dbo.Documents", "Id");
        }
    }
}
