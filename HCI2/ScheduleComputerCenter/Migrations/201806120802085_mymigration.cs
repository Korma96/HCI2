namespace ScheduleComputerCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mymigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classrooms", "Code", c => c.String());
            AddColumn("dbo.Softwares", "Code", c => c.String());
            AddColumn("dbo.Courses", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Code");
            DropColumn("dbo.Softwares", "Code");
            DropColumn("dbo.Classrooms", "Code");
        }
    }
}
