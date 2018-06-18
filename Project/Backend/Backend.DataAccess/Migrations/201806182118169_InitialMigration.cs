namespace Backend.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreetName = c.String(nullable: false),
                        StreetNumber = c.String(nullable: false),
                        City = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        YearOfManufactoring = c.Int(nullable: false),
                        RegistrationNumber = c.String(nullable: false),
                        TaxiNumber = c.String(nullable: false),
                        CarType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
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
                        CarId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.DriverLocationId, cascadeDelete: false)
                .Index(t => t.DriverLocationId);
            
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
                .ForeignKey("dbo.Rides", t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
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
                        CustomerId = c.Int(nullable: false),
                        DestinationLocationId = c.Int(nullable: false),
                        DispatcherId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        CommentId = c.Int(nullable: false),
                        RideStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.DestinationLocationId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.StartLocationId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.DispatcherId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.DriverId, cascadeDelete: false)
                .Index(t => t.StartLocationId)
                .Index(t => t.CustomerId)
                .Index(t => t.DestinationLocationId)
                .Index(t => t.DispatcherId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoordinateX = c.Double(nullable: false),
                        CoordinateY = c.Double(nullable: false),
                        AddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "Id", "dbo.Users");
            DropForeignKey("dbo.Rides", "DriverId", "dbo.Users");
            DropForeignKey("dbo.Rides", "DispatcherId", "dbo.Users");
            DropForeignKey("dbo.Rides", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "Id", "dbo.Rides");
            DropForeignKey("dbo.Rides", "StartLocationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "DestinationLocationId", "dbo.Locations");
            DropForeignKey("dbo.Users", "DriverLocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Locations", new[] { "AddressId" });
            DropIndex("dbo.Rides", new[] { "DriverId" });
            DropIndex("dbo.Rides", new[] { "DispatcherId" });
            DropIndex("dbo.Rides", new[] { "DestinationLocationId" });
            DropIndex("dbo.Rides", new[] { "CustomerId" });
            DropIndex("dbo.Rides", new[] { "StartLocationId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "Id" });
            DropIndex("dbo.Users", new[] { "DriverLocationId" });
            DropIndex("dbo.Cars", new[] { "Id" });
            DropTable("dbo.Locations");
            DropTable("dbo.Rides");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
            DropTable("dbo.Cars");
            DropTable("dbo.Addresses");
        }
    }
}
