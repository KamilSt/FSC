namespace FSC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoicedbmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceCounters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentType = c.String(),
                        Month = c.String(),
                        Year = c.String(),
                        Counter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNmuber = c.String(),
                        OrderId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateOfInvoice = c.DateTime(nullable: false),
                        FileName = c.String(),
                        File = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.InvoiceTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentType = c.String(),
                        Template = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceDocuments", "OrderId", "dbo.Orders");
            DropIndex("dbo.InvoiceDocuments", new[] { "OrderId" });
            DropTable("dbo.InvoiceTemplates");
            DropTable("dbo.InvoiceDocuments");
            DropTable("dbo.InvoiceCounters");
        }
    }
}
