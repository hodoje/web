using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.Models
{
    public class ChangeRideRequestModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int StartLocationId { get; set; }
        public LocationDto StartLocation { get; set; }
        public int? DestinationLocationId { get; set; }
        public LocationDto DestinationLocation { get; set; }
        public double Price { get; set; }
        public string RideStatus { get; set; }
        public string CarType { get; set; }
        public int? CustomerId { get; set; }
        public UserDto Customer { get; set; }
        public int? DispatcherId { get; set; }
        public UserDto Dispatcher { get; set; }
        public int? DriverId { get; set; }
        public UserDto Driver { get; set; }
        public int? CommentId { get; set; }
        public CommentDto Comment { get; set; }
    }
}