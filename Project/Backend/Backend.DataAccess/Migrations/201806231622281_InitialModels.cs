namespace Backend.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Rides", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartLocationId = c.Int(nullable: false),
                        CarType = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        DestinationLocationId = c.Int(nullable: false),
                        DispatcherId = c.Int(),
                        DriverId = c.Int(),
                        Price = c.Double(nullable: false),
                        CommentId = c.Int(),
                        RideStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.DispatcherId)
                .ForeignKey("dbo.Locations", t => t.DestinationLocationId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.StartLocationId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.DriverId)
                .Index(t => t.StartLocationId)
                .Index(t => t.CustomerId)
                .Index(t => t.DestinationLocationId)
                .Index(t => t.DispatcherId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        NationalIdentificationNumber = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(nullable: false),
                        IsBanned = c.Boolean(nullable: false),
                        Role = c.Int(nullable: false),
                        DriverLocationId = c.Int(),
                        Car_YearOfManufactoring = c.Int(),
                        Car_RegistrationNumber = c.String(),
                        Car_TaxiNumber = c.String(),
                        Car_CarType = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.DriverLocationId)
                .Index(t => t.DriverLocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address_StreetName = c.String(nullable: false),
                        Address_StreetNumber = c.String(nullable: false),
                        Address_City = c.String(nullable: false),
                        Address_PostalCode = c.String(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Id", "dbo.Rides");
            DropForeignKey("dbo.Rides", "DriverId", "dbo.Users");
            DropForeignKey("dbo.Rides", "StartLocationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "DestinationLocationId", "dbo.Locations");
            DropForeignKey("dbo.Users", "DriverLocationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "DispatcherId", "dbo.Users");
            DropForeignKey("dbo.Rides", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "DriverLocationId" });
            DropIndex("dbo.Rides", new[] { "DriverId" });
            DropIndex("dbo.Rides", new[] { "DispatcherId" });
            DropIndex("dbo.Rides", new[] { "DestinationLocationId" });
            DropIndex("dbo.Rides", new[] { "CustomerId" });
            DropIndex("dbo.Rides", new[] { "StartLocationId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "Id" });
            DropTable("dbo.Locations");
            DropTable("dbo.Users");
            DropTable("dbo.Rides");
            DropTable("dbo.Comments");
        }
    }
}
