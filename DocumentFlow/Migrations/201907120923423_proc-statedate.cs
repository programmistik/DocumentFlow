namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class procstatedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskProcesses", "StateDate", c => c.DateTime());
            DropColumn("dbo.TaskProcesses", "FinishDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskProcesses", "FinishDate", c => c.DateTime());
            DropColumn("dbo.TaskProcesses", "StateDate");
        }
    }
}
