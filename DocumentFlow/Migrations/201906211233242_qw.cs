namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qw : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskProcesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        Comment = c.String(),
                        FinishDate = c.DateTime(),
                        Department_Id = c.Int(),
                        StartUser_Id = c.Int(),
                        State_Id = c.Int(),
                        TaskUser_Id = c.Int(),
                        Document_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.Users", t => t.StartUser_Id)
                .ForeignKey("dbo.DocumentStates", t => t.State_Id)
                .ForeignKey("dbo.Users", t => t.TaskUser_Id)
                .ForeignKey("dbo.Documents", t => t.Document_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.StartUser_Id)
                .Index(t => t.State_Id)
                .Index(t => t.TaskUser_Id)
                .Index(t => t.Document_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskProcesses", "Document_Id", "dbo.Documents");
            DropForeignKey("dbo.TaskProcesses", "TaskUser_Id", "dbo.Users");
            DropForeignKey("dbo.TaskProcesses", "State_Id", "dbo.DocumentStates");
            DropForeignKey("dbo.TaskProcesses", "StartUser_Id", "dbo.Users");
            DropForeignKey("dbo.TaskProcesses", "Department_Id", "dbo.Departments");
            DropIndex("dbo.TaskProcesses", new[] { "Document_Id" });
            DropIndex("dbo.TaskProcesses", new[] { "TaskUser_Id" });
            DropIndex("dbo.TaskProcesses", new[] { "State_Id" });
            DropIndex("dbo.TaskProcesses", new[] { "StartUser_Id" });
            DropIndex("dbo.TaskProcesses", new[] { "Department_Id" });
            DropTable("dbo.TaskProcesses");
        }
    }
}
