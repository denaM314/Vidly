namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenreTableAndSeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            Sql("INSERT INTO Genres VALUES ('Action')");
            Sql("INSERT INTO Genres VALUES ('Drama')");
            Sql("INSERT INTO Genres VALUES ('Comedy')");
            Sql("INSERT INTO Genres VALUES ('Romance')");
            Sql("INSERT INTO Genres VALUES ('Thriller')");
            Sql("INSERT INTO Genres VALUES ('Horror')");
        }

        
        public override void Down()
        {
            DropTable("dbo.Genres");
        }
    }
}
