import { Comment } from './../../models/comment.model';
import { LocationsService } from './../../services/locations.service';
import { RidesService } from './../../services/rides.service';
import { CarsService } from './../../services/cars.service';
import { Location } from './../../models/location.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ApiMessage } from '../../models/apiMessage.model';
import { LoginModel } from '../../models/login.model';
import { User } from '../../models/user.model';
import { RegistrationModel } from '../../models/registration.model';
import { Car } from '../../models/car.model';
import { Ride } from '../../models/ride.model';
import { FormGroup, FormControl } from '@angular/forms';
import { RideStatus } from '../../models/rideStatus';
import { DispatcherProcessRideRequest } from '../../models/dispatcherProcessRideRequest';

declare var jQuery: any;

@Component({
  selector: 'driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.scss']
})
export class DriverComponent implements OnInit {

  personalData: User;
  driverRides: Ride[];
  pendingRides: Ride[];
  takenRide: Ride;

  rideStatuses: string[];
  isRideRequestPending = false;
  isRideChanging = false;
  isRideCancelled = false;
  ratingList = [false, false, false, false, false];
  
  personalDataForm = new FormGroup({
    username: new FormControl(),
    password: new FormControl(),
    name: new FormControl(),
    lastname: new FormControl(),
    email: new FormControl(),
    gender: new FormControl(),
    nationalIdentificationNumber: new FormControl(),
    phoneNumber: new FormControl()
  });

  carDataForm = new FormGroup({
    taxiNumber: new FormControl(),
    registrationNumber: new FormControl(),
    yearOfManufactoring: new FormControl(),
    carType: new FormControl()
  });

  locationDataForm = new FormGroup({
    address: new FormGroup({
      streetName: new FormControl(),
      streetNumber: new FormControl(),
      city: new FormControl(),
      postalCode: new FormControl()
    }),
    longitude: new FormControl(),
    latitude: new FormControl()
  });

  refineForm = new FormGroup({
    filter: new FormControl(),
    sort: new FormGroup({
      byDate: new FormControl(),
      byRating: new FormControl()
    }),
    search: new FormGroup({
      byDate: new FormGroup({
        from: new FormControl(),
        to: new FormControl()
      }),
      byRating: new FormGroup({
        from: new FormControl(),
        to: new FormControl()
      }),
      byPrice: new FormGroup({
        from: new FormControl(),
        to: new FormControl()
      }),
    })
  });

  successfulRideForm = new FormGroup({
    destinationLocation: new FormGroup({
      address: new FormGroup({
        streetName: new FormControl(),
        streetNumber: new FormControl(),
        city: new FormControl(),
        postalCode: new FormControl()
      }),
      longitude: new FormControl(),
      latitude: new FormControl()
    }),
    price: new FormControl(),
    commentDescription: new FormControl()
  });

  failedRideForm = new FormGroup({
    commentDescription: new FormControl()
  });

  acceptedRideForm = new FormGroup({
    // location: new FormGroup({
    //   address: new FormGroup({
    //     streetName: new FormControl(),
    //     streetNumber: new FormControl(),
    //     city: new FormControl(),
    //     postalCode: new FormControl()
    //   }),
    //   longitude: new FormControl(),
    //   latitude: new FormControl()
    // }),
    // carType: new FormControl(),
    // driverId: new FormControl(),
    price: new FormControl(),
    // rating: new FormControl(),
    commentDescription: new FormControl()
  });

  constructor(private usersService: UsersService, 
              private ridesService: RidesService, 
              private carsService: CarsService, 
              private locationsService: LocationsService) { }

  ngOnInit() {
    this.personalData = new User();
    this.driverRides = [];
    this.rideStatuses = [];
    this.pendingRides = [];    
    this.getMyData();
    this.getAllDriverRides();
    this.getAllPendingRides();
  }

  get pdForm(){
    return this.personalDataForm.controls;
  }

  get cdForm(){
    return this.carDataForm.controls;
  }

  get ldForm(){
    return this.locationDataForm.controls;
  }

  getMyData(){
    this.usersService.getUserByUsername().subscribe(
    (data: User) =>{
      this.personalData = data;
      this.personalDataForm.patchValue({
        username: data.username,
        password: data.password,
        name: data.name,
        lastname: data.lastname,
        email: data.email,
        gender: data.gender,
        nationalIdentificationNumber: data.nationalIdentificationNumber,
        phoneNumber: data.phoneNumber
      });

      this.carDataForm.patchValue({
        taxiNumber: data.car.taxiNumber,
        registrationNumber: data.car.registrationNumber,
        yearOfManufactoring: data.car.yearOfManufactoring,
        carType: data.car.carType
      });

      if(data.driverLocation !== null){
        this.locationDataForm.patchValue({
          address: data.driverLocation.address,
          longitude: data.driverLocation.longitude,
          latitude: data.driverLocation.latitude
        });
      }
      
      this.personalDataForm.markAsPristine();
      this.carDataForm.markAsPristine();
      this.locationDataForm.markAsPristine();
    });
  }

  changeMyData(){
    let updatedUser = new User();
    updatedUser =  this.personalDataForm.value;
    updatedUser.id = this.personalData.id;
    updatedUser.isBanned = this.personalData.isBanned;
    updatedUser.role = this.personalData.role;

    let apiMessage = new ApiMessage(localStorage.userHash, updatedUser);

    this.usersService.put(updatedUser.id, apiMessage).subscribe(
      (data: string) => {
        localStorage.userHash = data;
        this.personalDataForm.markAsPristine();
      },
      error => {
        console.log(error);
      }
    );
  }

  changeCarData(){
    let updatedCar = new Car();
    updatedCar = this.carDataForm.value;
    updatedCar.id = this.personalData.car.id;
    updatedCar.driverId = this.personalData.id;
    this.carsService.updateCar(updatedCar).subscribe(
      () => {
        this.carDataForm.patchValue({
          taxiNumber: updatedCar.taxiNumber,
          registrationNumber: updatedCar.registrationNumber,
          yearOfManufactoring: updatedCar.yearOfManufactoring,
          carType: updatedCar.carType
        });
        this.carDataForm.markAsPristine();
      }
    );
  }

  changeLocationData(){
    let updatedLocation = new Location();
    updatedLocation = this.locationDataForm.value;
    if(this.personalData.driverLocationId === null || this.personalData.driverLocationId === undefined){
      updatedLocation.id = -1;
    }
    else{
      updatedLocation.id = this.personalData.driverLocationId;
    }
    this.locationsService.addOrupdateDriverLocation(updatedLocation).subscribe(
      () => {
        this.locationDataForm.patchValue({
          address: updatedLocation.address,
          longitude: updatedLocation.longitude,
          latitude: updatedLocation.latitude
        });
        this.locationDataForm.markAsPristine();
      }
    );
  }

  private parseSingleRide(unparsedRide: Ride){
    let parsedRide = unparsedRide;
    let tempdate = new Date(unparsedRide.timestamp);
    parsedRide.timestamp = `${tempdate.toLocaleDateString()} ${tempdate.toLocaleTimeString()}`;
    if(parsedRide.comments.length > 0){
      parsedRide.comments.forEach(c => {
        c.timestamp = new Date(c.timestamp);  
      })
    }
    return parsedRide;
  }

  private parseRides(unparsedRides: Ride[]){
    let parsedRides = unparsedRides;
    parsedRides.forEach(r => {
      r = this.parseSingleRide(r);
    });
    return parsedRides;
  }

  private setCurrentUserCommentToBottom(commentsArray: Comment[]){
    if(commentsArray.length > 1){
      let commentToRemove = commentsArray.find(c => c.user.username === this.personalData.username);
      let index = commentsArray.indexOf(commentToRemove);
      commentsArray.splice(index, 1);
      commentsArray.push(commentToRemove);
    }
    return commentsArray;
  }

  getAllDriverRides(){
    this.ridesService.getAllDriverRides().subscribe(
      (data: Ride[]) => {
        this.driverRides = this.parseRides(data);
        this.driverRides.forEach(r => {
          r.comments = this.setCurrentUserCommentToBottom(r.comments);
        });
        this.takenRide = this.driverRides.find(r => r.rideStatus === 'ACCEPTED' && r.driver.id === this.personalData.id);
      }
    );
  }

  getAllPendingRides(){
    this.ridesService.getAllPendingRides().subscribe(
      (data: Ride[]) => {
        this.pendingRides = this.parseRides(data);
      }
    );
  }

  takeOverARide(rideId){
    let rideToTakeOver = this.pendingRides.find(r => r.id === rideId);
    let rideToTakeOverIndex = this.pendingRides.indexOf(rideToTakeOver);
    this.pendingRides.splice(rideToTakeOverIndex, 1);
    
    // Using the same model as dispatcher process because we're sending the same data
    let driverTakeOverRequest = new DispatcherProcessRideRequest();
    driverTakeOverRequest.driverId = this.personalData.id;
    driverTakeOverRequest.rideId = rideId;
    this.ridesService.driverTakeOverRide(driverTakeOverRequest).subscribe(
      (data: Ride) => {
        this.takenRide = this.parseSingleRide(data);
      }
    );
  }

  finishSuccessfulRide(){
    let sRideValue = this.successfulRideForm.value;
    let ride = new Ride();
    ride.id = this.takenRide.id;
    ride.destinationLocation = sRideValue.destinationLocation;
    ride.price = sRideValue.price;

    if(sRideValue.commentDescription !== null){
      let comment = new Comment();
      comment.description = sRideValue.commentDescription;
      ride.comments = [];
      ride.comments.push(comment);
    }
    this.ridesService.finishSuccessfulRide(ride).subscribe(
      (data: Ride) => {
        let takenRideToUpdate = this.pendingRides.find(r => r.id === this.takenRide.id);
        let takenRideToUpdateIndex = this.pendingRides.indexOf(takenRideToUpdate);
        this.driverRides[takenRideToUpdateIndex] = this.parseSingleRide(data);
        this.takenRide = null;
        jQuery("#successfulRideModal").modal("toggle");
      }
    );
  }

  finishFailedRide(){
    let driverComment = new Comment();
    driverComment.rideId = this.takenRide.id;
    driverComment.description = this.failedRideForm.value.commentDescription;
    this.ridesService.finishFailedRide(driverComment).subscribe(
      (data: Ride) => {
        let takenRideToUpdate = this.pendingRides.find(r => r.id === this.takenRide.id);
        let takenRideToUpdateIndex = this.pendingRides.indexOf(takenRideToUpdate);
        this.driverRides[takenRideToUpdateIndex] = this.parseSingleRide(data);
        this.takenRide = null;
        jQuery("#failedRideModal").modal("toggle");
      }
    );
  }

  // commentCancelledRide(comment){
  //   jQuery("#commentModal").modal("toggle");
  //   this.isRideCancelled = !this.isRideCancelled;

  //   let newComment = new Comment();
  //   newComment.description = comment.description;
  //   newComment.timestamp = new Date();
  //   this.ridesService.commentCancelledRide(newComment).subscribe();
  // }

  addComment(comment: Comment){
    comment.id = comment.rideId;
    this.ridesService.addComment(comment).subscribe(
      (data: Ride) => {
        var rideIndex = this.driverRides.findIndex(r => r.id === data.id);
        this.driverRides[rideIndex] = this.parseSingleRide(data);
      }
    );
  }

  rate(rideId, ratingIndex){
    for(var i in this.ratingList){
      if(i <= ratingIndex){
        this.ratingList[i] = true;
      }
      else{
        this.ratingList[i] = false;
      }
    }
    let rideToComment = this.driverRides.find(r => r.id === rideId);
    if(rideToComment.comments.length === 0){
      let comment = new Comment();
      comment.rating = ratingIndex + 1;
      comment.rideId = rideId;
      comment.userId = this.personalData.id;
      this.ridesService.addComment(comment).subscribe(
        (data: Ride) => {
          var rideIndex = this.driverRides.findIndex(r => r.id === data.id);
        this.driverRides[rideIndex] = this.parseSingleRide(data);
        }
      );
    }
    else{
      let comment = rideToComment.comments.find(c => c.userId === this.personalData.id);  
      comment.rating = ratingIndex + 1;
      this.ridesService.rateARide(comment).subscribe();  
    }
  }

  // useHub(input){
  //   this.notificationService.broadcastMessage(input.field);
  // }
}
