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

@Component({
  selector: 'driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.scss']
})
export class DriverComponent implements OnInit {

  personalData: User;
  shouldDisplayPersonalSaveChanges = false;
  shouldDisplayCarSaveChanges = false;
  shouldDisplayLocationSaveChanges = false;
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

  takeOverARideForm = new FormGroup({
    driverId: new FormControl()
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
        yearOfManufactoring: data.car.yearOfManufactoring;
        carType: data.car.carType
      });

      this.shouldDisplayPersonalSaveChanges = !this.shouldDisplayPersonalSaveChanges;
      this.shouldDisplayCarSaveChanges = !this.shouldDisplayCarSaveChanges;
      this.shouldDisplayLocationSaveChanges = !this.shouldDisplayLocationSaveChanges;
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
        this.shouldDisplayPersonalSaveChanges = !this.shouldDisplayPersonalSaveChanges;
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
    
  }

  changeLocationData(){
    console.log(this.locationDataForm);
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

  getAllDriverRides(){
    this.ridesService.getAllDriverRides().subscribe(
      (data: Ride[]) => {
        this.driverRides = this.parseRides(data);
      }
    );
  }

  getAllPendingRides(){
    this.ridesService.getAllPendingRides().subscribe(
      (data: Ride[]) => {
        this.pendingRides = this.parseRides(data);
        this.takenRide = this.pendingRides.find(r => r.rideStatus === 'ACCEPTED' && r.driver.id === this.personalData.id);
      }
    );
  }

  takeOverARide(rideId){
    let rideToTakeOver = this.pendingRides.find(r => r.id === rideId);
    let rideToTakeOverIndex = this.pendingRides.indexOf(rideToTakeOver);
    //this.pendingRides.splice(rideToTakeOverIndex, 1);
    this.takenRide = rideToTakeOver;
    this.takenRide.rideStatus = RideStatus.ACCEPTED;
    console.log(this.takenRide);
  }

  // commentCancelledRide(comment){
  //   jQuery("#commentModal").modal("toggle");
  //   this.isRideCancelled = !this.isRideCancelled;

  //   let newComment = new Comment();
  //   newComment.description = comment.description;
  //   newComment.timestamp = new Date();
  //   this.ridesService.commentCancelledRide(newComment).subscribe();
  // }

  // addComment(comment: Comment){
  //   comment.id = comment.rideId;
  //   comment.timestamp = new Date();
  //   this.ridesService.addComment(comment).subscribe();
  // }

  // rate(rideId, ratingIndex){
  //   for(var i in this.ratingList){
  //     if(i <= ratingIndex){
  //       this.ratingList[i] = true;
  //     }
  //     else{
  //       this.ratingList[i] = false;
  //     }
  //   }
  //   let comment = this.ridesHistory.filter(r => r.id === rideId)[0].comment;
  //   comment.rating = ratingIndex + 1;
  //   this.ridesService.rateARide(comment).subscribe(
  //     (data) => {
  //     }
  //   );
  // }

  // useHub(input){
  //   this.notificationService.broadcastMessage(input.field);
  // }
}
