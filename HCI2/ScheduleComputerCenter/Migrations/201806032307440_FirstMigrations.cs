namespace ScheduleComputerCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Terms", "RowSpan", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Terms", "RowSpan");
        }
    }
}
