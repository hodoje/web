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
                        CarTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarTypes", t => t.CarTypeId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CarTypeId);
            
            CreateTable(
                "dbo.CarTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartLocationId = c.Int(nullable: false),
                        CarTypeId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DestinationLocationId = c.Int(nullable: false),
                        DispatcherId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        CommentId = c.Int(nullable: false),
                        RideStatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarTypes", t => t.CarTypeId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.DispatcherId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.DriverId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.DestinationLocationId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.StartLocationId, cascadeDelete: false)
                .ForeignKey("dbo.RideStatus", t => t.RideStatusId, cascadeDelete: false)
                .Index(t => t.StartLocationId)
                .Index(t => t.CarTypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.DestinationLocationId)
                .Index(t => t.DispatcherId)
                .Index(t => t.DriverId)
                .Index(t => t.RideStatusId);
            
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        GenderId = c.Int(nullable: false),
                        NationalIdentificationNumber = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        IsBanned = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                        DriverLocationId = c.Int(),
                        CarId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genders", t => t.GenderId, cascadeDelete: false)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.DriverLocationId, cascadeDelete: false)
                .Index(t => t.GenderId)
                .Index(t => t.RoleId)
                .Index(t => t.DriverLocationId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenderName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.RideStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "Id", "dbo.Users");
            DropForeignKey("dbo.Rides", "RideStatusId", "dbo.RideStatus");
            DropForeignKey("dbo.Rides", "StartLocationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "DestinationLocationId", "dbo.Locations");
            DropForeignKey("dbo.Users", "DriverLocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.Rides", "DriverId", "dbo.Users");
            DropForeignKey("dbo.Rides", "DispatcherId", "dbo.Users");
            DropForeignKey("dbo.Rides", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "Id", "dbo.Rides");
            DropForeignKey("dbo.Rides", "CarTypeId", "dbo.CarTypes");
            DropForeignKey("dbo.Cars", "CarTypeId", "dbo.CarTypes");
            DropIndex("dbo.Locations", new[] { "AddressId" });
            DropIndex("dbo.Users", new[] { "DriverLocationId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", new[] { "GenderId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "Id" });
            DropIndex("dbo.Rides", new[] { "RideStatusId" });
            DropIndex("dbo.Rides", new[] { "DriverId" });
            DropIndex("dbo.Rides", new[] { "DispatcherId" });
            DropIndex("dbo.Rides", new[] { "DestinationLocationId" });
            DropIndex("dbo.Rides", new[] { "CustomerId" });
            DropIndex("dbo.Rides", new[] { "CarTypeId" });
            DropIndex("dbo.Rides", new[] { "StartLocationId" });
            DropIndex("dbo.Cars", new[] { "CarTypeId" });
            DropIndex("dbo.Cars", new[] { "Id" });
            DropTable("dbo.RideStatus");
            DropTable("dbo.Locations");
            DropTable("dbo.Roles");
            DropTable("dbo.Genders");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Rides");
            DropTable("dbo.CarTypes");
            DropTable("dbo.Cars");
            DropTable("dbo.Addresses");
        }
    }
}
