import { DispatcherFormRideRequest } from './../../models/dispatcherFormRideRequest';
import { AccessService } from './../../services/access.service';
import { RidesService } from './../../services/rides.service';
import { RegistrationModel } from './../../models/registration.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user.model';
import { Car } from '../../models/car.model';
import { Location } from './../../models/location.model';
import { Ride } from '../../models/ride.model';
import { FormGroup, FormControl } from '@angular/forms';
import { DispatcherProcessRideRequest } from '../../models/dispatcherProcessRideRequest';
import { ApiMessage } from '../../models/apiMessage.model';

declare var jQuery: any;

@Component({
  selector: 'dispatcher',
  templateUrl: './dispatcher.component.html',
  styleUrls: ['./dispatcher.component.scss']
})
export class DispatcherComponent implements OnInit {

  personalData: User;
  allDrivers: User[];
  allRides: Ride[];
  allUsers: User[];
  dispatcherRides: Ride[];
  pendingRides: Ride[];
  ratingList = [false, false, false, false, false];
  shouldDisplaySaveChanges = false;

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

  rideForm = new FormGroup({
    location: new FormGroup({
      address: new FormGroup({
        streetName: new FormControl(),
        streetNumber: new FormControl(),
        city: new FormControl(),
        postalCode: new FormControl()
      }),
      longitude: new FormControl(),
      latitude: new FormControl()
    }),
    carType: new FormControl(),
    driverId: new FormControl()
  });  

  driverForm = new FormGroup({
    username: new FormControl(),
    password: new FormControl(),
    name: new FormControl(),
    lastname: new FormControl(),
    email: new FormControl(),
    gender: new FormControl(),
    nationalIdentificationNumber: new FormControl(),
    phoneNumber: new FormControl(),
    car: new FormGroup({
      registrationNumber: new FormControl(),
      taxiNumber: new FormControl(),
      yearOfManufactoring: new FormControl(),
      carType: new FormControl()
    })
  });

  processARideForm = new FormGroup({
    driverId: new FormControl()
  });

  dispatcherRidesSearchForm = new FormGroup({
    userType: new FormControl(),
    name: new FormControl(),
    lastname: new FormControl()
  });

  constructor(private usersService: UsersService, private ridesService: RidesService, private accessService: AccessService) {
  }

  ngOnInit() {
    this.personalData = new User();
    this.allDrivers = [];
    this.allRides = [];
    this.allUsers = [];
    this.pendingRides = [];
    this.dispatcherRides = [];
    this.getMyData();
    this.getAllRides();
    this.getAllUsers();
    this.getAllDrivers();
    this.getAllDispatcherRides();
    this.getAllPendingRides();
  }

  get pdForm(){
    return this.personalDataForm.controls;
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
      this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
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
        this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
      },
      error => {
        console.log(error);
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

  getAllRides(){
    this.ridesService.getAllRides().subscribe(
      (data: Ride[]) => {
        this.allRides = this.parseRides(data);
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

  getAllDispatcherRides(){
    this.ridesService.getAllDispatcherRides().subscribe(
      (data: Ride[]) => {
        this.dispatcherRides = this.parseRides(data);
      }
    );
  }

  getAllUsers(){
    this.usersService.getAllUsers().subscribe(
      (data: User[]) => {
        this.allUsers = data;
      }
    );
  }

  getAllDrivers(){
    this.usersService.getAllDrivers().subscribe(
      (data: User[]) => {
        this.allDrivers = data;
      }
    );
  }

  addDriver(){
    let newDriver = new User();
    newDriver = this.driverForm.value;
    newDriver.role = 'DRIVER';
    this.usersService.post(newDriver).subscribe(
      data => {
        jQuery("#addADriverModal").modal("toggle");
        this.driverForm.reset();
        this.getAllUsers();
      }
    );
  }

  blockUser(usernameToBlock){
    this.accessService.blockUser(usernameToBlock).subscribe(
      () => {
        let driver = this.allUsers.find(u => u.username === usernameToBlock);
        driver.isBanned = true;
      }
    );
  }

  unblockUser(usernameToUnblock){
    this.accessService.unblockUser(usernameToUnblock).subscribe(
      () => {
        let driver = this.allUsers.find(u => u.username === usernameToUnblock);
        driver.isBanned = false;
      }
    );
  }

  searchRides(){
    this.ridesService.dispatcherRidesSearch(this.dispatcherRidesSearchForm.value).subscribe(
      (data: Ride[]) => {
        this.allRides = this.parseRides(data);
      }
    );
  }

  formARide(){
    let formRideRequest = new DispatcherFormRideRequest();
    formRideRequest = this.rideForm.value;
    formRideRequest.dispatcherId = this.personalData.id;
    this.ridesService.formARide(formRideRequest).subscribe(
      () => {
        jQuery("#formARideModal").modal("toggle");
        this.rideForm.reset();
        this.getAllRides();
        this.getAllDispatcherRides();
      }
    );
  }

  startProcessingARide(rideId){
    let selectedRide = this.pendingRides.find(r => r.id === rideId);
  }

  processARide(rideId){
    jQuery("#processARideModal").modal("toggle");
    let processARideRequest = new DispatcherProcessRideRequest();
    processARideRequest = this.processARideForm.value;
    processARideRequest.dispatcherId = this.personalData.id;
    processARideRequest.rideId = rideId;
    this.ridesService.processARide(processARideRequest).subscribe(
      () => {
        this.getAllRides();
        this.getAllDispatcherRides();
        this.getAllPendingRides();
      }
    );
  }
}
