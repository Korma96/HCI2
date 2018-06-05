namespace ScheduleComputerCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Code");
        }
    }
}
