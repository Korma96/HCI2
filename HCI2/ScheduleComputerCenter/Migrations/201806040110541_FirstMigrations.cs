namespace ScheduleComputerCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigrations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classrooms", "Software_Id", "dbo.Softwares");
            DropIndex("dbo.Classrooms", new[] { "Software_Id" });
            AddColumn("dbo.Softwares", "Classroom_Id", c => c.Int());
            AddColumn("dbo.Courses", "Mark", c => c.String());
            CreateIndex("dbo.Softwares", "Classroom_Id");
            AddForeignKey("dbo.Softwares", "Classroom_Id", "dbo.Classrooms", "Id");
            DropColumn("dbo.Classrooms", "Software_Id");
            DropColumn("dbo.Subjects", "Mark");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Subjects", "Mark", c => c.String());
            AddColumn("dbo.Classrooms", "Software_Id", c => c.Int());
            DropForeignKey("dbo.Softwares", "Classroom_Id", "dbo.Classrooms");
            DropIndex("dbo.Softwares", new[] { "Classroom_Id" });
            DropColumn("dbo.Courses", "Mark");
            DropColumn("dbo.Softwares", "Classroom_Id");
            CreateIndex("dbo.Classrooms", "Software_Id");
            AddForeignKey("dbo.Classrooms", "Software_Id", "dbo.Softwares", "Id");
        }
    }
}
