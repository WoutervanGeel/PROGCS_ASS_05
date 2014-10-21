namespace Prog5Assessment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomPrices : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomPrices", "DateStart", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomPrices", "DateEnd", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomPrices", "DateEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CustomPrices", "DateStart", c => c.DateTime(nullable: false));
        }
    }
}
