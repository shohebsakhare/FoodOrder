namespace FoodOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminLogins",
                c => new
                    {
                        adminid = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.adminid);
            
            CreateTable(
                "dbo.BlogModels",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        BlogTitle = c.String(nullable: false),
                        BlogDate = c.DateTime(nullable: false),
                        BlogAutherName = c.String(nullable: false),
                        BlogDescription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BlogId);
            
            CreateTable(
                "dbo.ContactModels",
                c => new
                    {
                        contactId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        Subject = c.String(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.contactId);
            
            CreateTable(
                "dbo.InvoiceModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateInvoice = c.DateTime(),
                        Total_Bill = c.Single(nullable: false),
                        FKUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SignupLogins", t => t.FKUserID)
                .Index(t => t.FKUserID);
            
            CreateTable(
                "dbo.SignupLogins",
                c => new
                    {
                        userid = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.userid);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qty = c.Int(nullable: false),
                        Unit_Price = c.Int(nullable: false),
                        Order_Bill = c.Single(nullable: false),
                        Order_Date = c.DateTime(),
                        FkProdId = c.Int(),
                        FkInvoiceID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoiceModels", t => t.FkInvoiceID)
                .ForeignKey("dbo.Products", t => t.FkProdId)
                .Index(t => t.FkProdId)
                .Index(t => t.FkInvoiceID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductPrice = c.Int(nullable: false),
                        ProductPicture = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "FkProdId", "dbo.Products");
            DropForeignKey("dbo.Orders", "FkInvoiceID", "dbo.InvoiceModels");
            DropForeignKey("dbo.InvoiceModels", "FKUserID", "dbo.SignupLogins");
            DropIndex("dbo.Orders", new[] { "FkInvoiceID" });
            DropIndex("dbo.Orders", new[] { "FkProdId" });
            DropIndex("dbo.InvoiceModels", new[] { "FKUserID" });
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.SignupLogins");
            DropTable("dbo.InvoiceModels");
            DropTable("dbo.ContactModels");
            DropTable("dbo.BlogModels");
            DropTable("dbo.AdminLogins");
        }
    }
}
