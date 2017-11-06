namespace FSC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SQLFUNCTION : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.OrderItems ADD Gross AS CAST(Quantity * Rate * ((VAT/100)+1) AS Decimal(18, 2))");


            Sql(@"CREATE FUNCTION dbo.GetSumOfOrderItems(@OrderId INT)
                RETURNS DECIMAL(18, 2)
                AS
                BEGIN

                DECLARE @total Decimal(18, 2);

                SELECT @total = Sum(Gross)
                FROM OrderItems
                WHERE OrderId = @OrderId;

                RETURN ISNULL(@total, 0);
                END");

            Sql("ALTER TABLE dbo.Orders ADD Total AS dbo.GetSumOfOrderItems(OrderId)");
            //AddColumn("dbo.Orders", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            Sql("ALTER TABLE  dbo.OrderItems  DROP COLUMN Gross");
            Sql("ALTER TABLE  dbo.Orders  DROP COLUMN Total");
        }
    }
}
