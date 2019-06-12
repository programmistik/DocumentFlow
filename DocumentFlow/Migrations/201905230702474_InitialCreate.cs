namespace DocumentFlow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ColorSchemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Constants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocPath = c.String(),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Value = c.String(),
                        ContactInfoType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.ContactId, cascadeDelete: true)
                .ForeignKey("dbo.ContactInfoTypes", t => t.ContactInfoType_Id)
                .Index(t => t.ContactId)
                .Index(t => t.ContactInfoType_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Photo = c.String(),
                        UserId = c.Int(),
                        CompanyId = c.Int(),
                        DepartmentId = c.Int(),
                        PositionId = c.Int(),
                        LanguageId = c.Int(),
                        ColorSchemeId = c.Int(),
                        HeadOfDep = c.Boolean(),
                        CanEditContacts = c.Boolean(),
                        OrganizationId = c.Int(),
                        Account = c.String(),
                        Comment = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColorSchemes", t => t.ColorSchemeId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CompanyId)
                .Index(t => t.DepartmentId)
                .Index(t => t.PositionId)
                .Index(t => t.LanguageId)
                .Index(t => t.ColorSchemeId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LangCode = c.String(),
                        LangCultureCode = c.String(),
                        LangName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        GoogleAccount = c.String(),
                        SaltValue = c.String(),
                        HashValue = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInfoTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InfoType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocStateName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MyFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileUri = c.String(),
                        DocId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocId, cascadeDelete: true)
                .Index(t => t.DocId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocNumber = c.String(),
                        DocDate = c.DateTime(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        DocumentTypeId = c.Int(nullable: false),
                        DocumentStateId = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CrUserId = c.Int(nullable: false),
                        ModifyDate = c.DateTime(nullable: false),
                        MdUserId = c.Int(nullable: false),
                        Comment = c.String(),
                        CreatedBy_Id = c.Int(),
                        ModifyBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.DocumentStates", t => t.DocumentStateId, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ModifyBy_Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.DepartmentId)
                .Index(t => t.DocumentTypeId)
                .Index(t => t.DocumentStateId)
                .Index(t => t.OrganizationId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.ModifyBy_Id);
            
            CreateTable(
                "dbo.NewsPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostContent = c.String(),
                        PostHeader = c.String(),
                        PostEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.MyFiles", "DocId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "ModifyBy_Id", "dbo.Users");
            DropForeignKey("dbo.Documents", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "DocumentStateId", "dbo.DocumentStates");
            DropForeignKey("dbo.Documents", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Documents", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Documents", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ContactInformations", "ContactInfoType_Id", "dbo.ContactInfoTypes");
            DropForeignKey("dbo.Contacts", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Contacts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Contacts", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Contacts", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Contacts", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Contacts", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Contacts", "ColorSchemeId", "dbo.ColorSchemes");
            DropForeignKey("dbo.ContactInformations", "ContactId", "dbo.Contacts");
            DropIndex("dbo.Documents", new[] { "ModifyBy_Id" });
            DropIndex("dbo.Documents", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Documents", new[] { "OrganizationId" });
            DropIndex("dbo.Documents", new[] { "DocumentStateId" });
            DropIndex("dbo.Documents", new[] { "DocumentTypeId" });
            DropIndex("dbo.Documents", new[] { "DepartmentId" });
            DropIndex("dbo.Documents", new[] { "CompanyId" });
            DropIndex("dbo.MyFiles", new[] { "DocId" });
            DropIndex("dbo.Contacts", new[] { "OrganizationId" });
            DropIndex("dbo.Contacts", new[] { "ColorSchemeId" });
            DropIndex("dbo.Contacts", new[] { "LanguageId" });
            DropIndex("dbo.Contacts", new[] { "PositionId" });
            DropIndex("dbo.Contacts", new[] { "DepartmentId" });
            DropIndex("dbo.Contacts", new[] { "CompanyId" });
            DropIndex("dbo.Contacts", new[] { "UserId" });
            DropIndex("dbo.ContactInformations", new[] { "ContactInfoType_Id" });
            DropIndex("dbo.ContactInformations", new[] { "ContactId" });
            DropTable("dbo.NewsPosts");
            DropTable("dbo.Documents");
            DropTable("dbo.MyFiles");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentStates");
            DropTable("dbo.ContactInfoTypes");
            DropTable("dbo.Organizations");
            DropTable("dbo.Users");
            DropTable("dbo.Positions");
            DropTable("dbo.Languages");
            DropTable("dbo.Departments");
            DropTable("dbo.Contacts");
            DropTable("dbo.ContactInformations");
            DropTable("dbo.Constants");
            DropTable("dbo.Companies");
            DropTable("dbo.ColorSchemes");
        }
    }
}
