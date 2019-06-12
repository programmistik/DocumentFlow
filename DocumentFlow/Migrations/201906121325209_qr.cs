namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Documents", "ModifyBy_Id", "dbo.Users");
            DropIndex("dbo.Documents", new[] { "DepartmentId" });
            DropIndex("dbo.Documents", new[] { "ModifyBy_Id" });
            AddColumn("dbo.MyFiles", "FileComment", c => c.String());
            AddColumn("dbo.Documents", "DocInfoDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Documents", "ContactId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "DocSum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Documents", "CurrencyId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "DocInfoComment", c => c.String());
            AddColumn("dbo.Documents", "InfoContact_Id", c => c.Int());
            CreateIndex("dbo.Documents", "CurrencyId");
            CreateIndex("dbo.Documents", "InfoContact_Id");
            AddForeignKey("dbo.Documents", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Documents", "InfoContact_Id", "dbo.Contacts", "Id");
            DropColumn("dbo.Documents", "DepartmentId");
            DropColumn("dbo.Documents", "CreateDate");
            DropColumn("dbo.Documents", "ModifyDate");
            DropColumn("dbo.Documents", "MdUserId");
            DropColumn("dbo.Documents", "ModifyBy_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "ModifyBy_Id", c => c.Int());
            AddColumn("dbo.Documents", "MdUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "ModifyDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Documents", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Documents", "DepartmentId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Documents", "InfoContact_Id", "dbo.Contacts");
            DropForeignKey("dbo.Documents", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Documents", new[] { "InfoContact_Id" });
            DropIndex("dbo.Documents", new[] { "CurrencyId" });
            DropColumn("dbo.Documents", "InfoContact_Id");
            DropColumn("dbo.Documents", "DocInfoComment");
            DropColumn("dbo.Documents", "CurrencyId");
            DropColumn("dbo.Documents", "DocSum");
            DropColumn("dbo.Documents", "ContactId");
            DropColumn("dbo.Documents", "DocInfoDate");
            DropColumn("dbo.MyFiles", "FileComment");
            CreateIndex("dbo.Documents", "ModifyBy_Id");
            CreateIndex("dbo.Documents", "DepartmentId");
            AddForeignKey("dbo.Documents", "ModifyBy_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
        }
    }
}
