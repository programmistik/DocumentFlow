namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deadline : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TaskProcesses", "Deadline");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskProcesses", "Deadline", c => c.DateTime(nullable: false));
        }
    }
}
