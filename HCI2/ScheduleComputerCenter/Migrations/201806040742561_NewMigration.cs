namespace ScheduleComputerCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        NumOfSeats = c.Int(nullable: false),
                        Projector = c.Boolean(nullable: false),
                        Table = c.Boolean(nullable: false),
                        SmartTable = c.Boolean(nullable: false),
                        OsType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Softwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OsType = c.Int(nullable: false),
                        Manufacturer = c.String(),
                        Website = c.String(),
                        YearOfFounding = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Description = c.String(),
                        Classroom_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classrooms", t => t.Classroom_Id)
                .Index(t => t.Classroom_Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateOfFounding = c.String(),
                        Description = c.String(),
                        Mark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        DayId = c.Int(nullable: false),
                        RowSpan = c.Int(nullable: false),
                        Subject_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .Index(t => t.DayId)
                .Index(t => t.Subject_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        NumOfStudents = c.Int(nullable: false),
                        MinNumOfClassesPerTerm = c.Int(nullable: false),
                        NumOfClasses = c.Int(nullable: false),
                        Projector = c.Boolean(nullable: false),
                        Table = c.Boolean(nullable: false),
                        SmartTable = c.Boolean(nullable: false),
                        OsType = c.Int(nullable: false),
                        Course_Id = c.Int(),
                        Software_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Softwares", t => t.Software_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Software_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Terms", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "Software_Id", "dbo.Softwares");
            DropForeignKey("dbo.Subjects", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Terms", "DayId", "dbo.Days");
            DropForeignKey("dbo.Softwares", "Classroom_Id", "dbo.Classrooms");
            DropIndex("dbo.Subjects", new[] { "Software_Id" });
            DropIndex("dbo.Subjects", new[] { "Course_Id" });
            DropIndex("dbo.Terms", new[] { "Subject_Id" });
            DropIndex("dbo.Terms", new[] { "DayId" });
            DropIndex("dbo.Softwares", new[] { "Classroom_Id" });
            DropTable("dbo.Subjects");
            DropTable("dbo.Terms");
            DropTable("dbo.Days");
            DropTable("dbo.Courses");
            DropTable("dbo.Softwares");
            DropTable("dbo.Classrooms");
        }
    }
}
