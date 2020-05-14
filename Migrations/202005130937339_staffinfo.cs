namespace PayrollSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class staffinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "ICNo", c => c.String());
            AddColumn("dbo.AspNetUsers", "EPFNo", c => c.String());
            AddColumn("dbo.AspNetUsers", "SocsoNo", c => c.String());
            AddColumn("dbo.AspNetUsers", "BasicSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "BasicSalary");
            DropColumn("dbo.AspNetUsers", "SocsoNo");
            DropColumn("dbo.AspNetUsers", "EPFNo");
            DropColumn("dbo.AspNetUsers", "ICNo");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
