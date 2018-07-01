namespace FSC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusFilters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FiltersStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilterName = c.String(),
                        Version = c.Int(nullable: false),
                        Filters = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FiltersStatus", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FiltersStatus", new[] { "UserId" });
            DropTable("dbo.FiltersStatus");
        }
    }
}
