import { CancelRideRequest } from './../../models/cancelRideRequest';
import { RidesService } from './../../services/rides.service';
import { Location } from './../../models/location.model';
import { Ride } from './../../models/ride.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ApiMessage } from '../../models/apiMessage.model';
import { LoginModel } from '../../models/login.model';
import { User } from '../../models/user.model';
import { RegistrationModel } from '../../models/registration.model';
import { NotificationService } from '../../services/notification.service';
import { RideStatus } from './../../models/rideStatus';
import { RideRequest } from './../../models/rideRequest';
import { FormGroup, FormControl } from '../../../../node_modules/@angular/forms';

// for modal hiding in callARide
declare var jQuery: any;

@Component({
  selector: 'customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {

  personalData: User;
  ridesHistory: Ride[];
  shouldDisplaySaveChanges = false;
  rideStatus: RideStatus;

  isRideRequestSent = false;
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
    carType: new FormControl()
  })

  constructor(
    private userService: UsersService, 
    private notificationService: NotificationService,
    private ridesService: RidesService) {
    this.ridesHistory = new Array as Ride[];
    this.getMyData();
    // let ride = new Ride();
    // ride.startLocation = new Location();
    // ride.startLocation.address.streetName = "narodnog fronta";
    // ride.startLocation.address.streetNumber = "6";
    // ride.carType = "PASSENGER";
    // let ride2 = new Ride();
    // ride2.startLocation = new Location();
    // ride2.startLocation.address.streetName = "narodnog fronta";
    // ride.startLocation.address.streetNumber = "6";
    // ride2.carType = "PASSENGER";
    // this.ridesHistory.push(ride);
    // this.ridesHistory.push(ride2);
  }

  ngOnInit() {
  }

  getMyData(){
    //if(!this.shouldDisplaySaveChanges){
      let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));
      this.userService.getUserByUsername(apiMessage).subscribe(
      (data: User) =>{
        this.personalData = data;
        this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
      },
      error => {
        //this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
      });
    //}
    //else{
    //  this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
    //}
  }

  changeMyData(registrationModel: RegistrationModel){
    let updatedUser = registrationModel as User;
    updatedUser.id = this.personalData.id;
    updatedUser.isBanned = this.personalData.isBanned;
    updatedUser.role = this.personalData.role;
    let apiMessage = new ApiMessage(localStorage.userHash, updatedUser);
    this.userService.put(updatedUser.id, apiMessage).subscribe(
      (data: string) => {
        localStorage.userHash = data;
        this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
      },
      error => {
        console.log(error);
      }
    );
  }

  callARide(rideRequest: RideRequest){
    jQuery("#exampleModalLong").modal("hide");
    console.log(rideRequest);
    this.ridesService.requestRide(rideRequest).subscribe((data: Ride) =>{
      console.log(data.startLocation);
    });
    this.isRideRequestSent = true;
    let newRide = new Ride();
    newRide.startLocation = rideRequest.location;
    newRide.carType = rideRequest.carType;
    this.ridesHistory.push(newRide);
  }

  cancelARide(){
    let cancelRideRequest = new CancelRideRequest(true);
    this.ridesService.cancelRide(cancelRideRequest).subscribe(data => {console.log(data)});
    this.isRideRequestSent = false;
  }

  useHub(input){
    console.log(input.field);
    this.notificationService.broadcastMessage(input.field);
  }
}
