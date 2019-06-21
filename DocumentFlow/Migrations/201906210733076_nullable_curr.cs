namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable_curr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Documents", new[] { "CurrencyId" });
            AlterColumn("dbo.Documents", "CurrencyId", c => c.Int());
            CreateIndex("dbo.Documents", "CurrencyId");
            AddForeignKey("dbo.Documents", "CurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Documents", new[] { "CurrencyId" });
            AlterColumn("dbo.Documents", "CurrencyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "CurrencyId");
            AddForeignKey("dbo.Documents", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
    }
}
