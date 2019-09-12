namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ttt : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TaskProcesses", name: "Department_Id", newName: "DepartmentId");
            RenameColumn(table: "dbo.TaskProcesses", name: "StartUser_Id", newName: "StartUserId");
            RenameColumn(table: "dbo.TaskProcesses", name: "State_Id", newName: "StateId");
            RenameColumn(table: "dbo.TaskProcesses", name: "TaskUser_Id", newName: "TaskUserId");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_StartUser_Id", newName: "IX_StartUserId");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_Department_Id", newName: "IX_DepartmentId");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_TaskUser_Id", newName: "IX_TaskUserId");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_State_Id", newName: "IX_StateId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_StateId", newName: "IX_State_Id");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_TaskUserId", newName: "IX_TaskUser_Id");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_DepartmentId", newName: "IX_Department_Id");
            RenameIndex(table: "dbo.TaskProcesses", name: "IX_StartUserId", newName: "IX_StartUser_Id");
            RenameColumn(table: "dbo.TaskProcesses", name: "TaskUserId", newName: "TaskUser_Id");
            RenameColumn(table: "dbo.TaskProcesses", name: "StateId", newName: "State_Id");
            RenameColumn(table: "dbo.TaskProcesses", name: "StartUserId", newName: "StartUser_Id");
            RenameColumn(table: "dbo.TaskProcesses", name: "DepartmentId", newName: "Department_Id");
        }
    }
}
