namespace ScheduleComputerCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Softwares", "Classroom_Id", "dbo.Classrooms");
            DropIndex("dbo.Softwares", new[] { "Classroom_Id" });
            AddColumn("dbo.Classrooms", "Software_Id", c => c.Int());
            AddColumn("dbo.Subjects", "Code", c => c.String());
            CreateIndex("dbo.Classrooms", "Software_Id");
            AddForeignKey("dbo.Classrooms", "Software_Id", "dbo.Softwares", "Id");
            DropColumn("dbo.Softwares", "Classroom_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Softwares", "Classroom_Id", c => c.Int());
            DropForeignKey("dbo.Classrooms", "Software_Id", "dbo.Softwares");
            DropIndex("dbo.Classrooms", new[] { "Software_Id" });
            DropColumn("dbo.Subjects", "Code");
            DropColumn("dbo.Classrooms", "Software_Id");
            CreateIndex("dbo.Softwares", "Classroom_Id");
            AddForeignKey("dbo.Softwares", "Classroom_Id", "dbo.Classrooms", "Id");
        }
    }
}
