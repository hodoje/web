import { Comment } from './../../models/comment.model';
import { ChangeRideRequest } from './../../models/changeRideRequest';
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
import { Address } from '../../models/address.model';

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
  rideStatuses: string[];
  isRideRequestPending = false;
  isRideChanging = false;
  isRideCancelled = false;
  ratingList = [false, false, false, false, false];

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

  constructor(private userService: UsersService, private notificationService: NotificationService, private ridesService: RidesService) {
    this.personalData = new User();
    this.ridesHistory = [];
    this.rideStatuses = [];
    let rideStatusEnumKeys = Object.keys(RideStatus);
    for(var s of rideStatusEnumKeys){
      this.rideStatuses.push(s);
    }
    this.getMyData();
    this.getAllMyRides();
  }

  ngOnInit() {
  }

  getMyData(){
    this.userService.getUserByUsername().subscribe(
    (data: User) =>{
      this.personalData = data;
      this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
    });
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
    jQuery("#callARideModal").modal("toggle");

    if(rideRequest.carType === null){
      rideRequest.carType = 'DEFAULT';
    }
    this.ridesService.requestRide(rideRequest).subscribe(
      (data: Ride) =>{
      
    });
    this.isRideRequestPending = true;
  }

  changeARide(changeRideRequest: ChangeRideRequest){
    jQuery("#callARideModal").modal("toggle");
    this.isRideRequestPending = !this.isRideRequestPending;
    this.isRideChanging = false;

    if(changeRideRequest.carType === null){
      changeRideRequest.carType = 'DEFAULT';
    }

    this.ridesService.changeRide(changeRideRequest).subscribe(
      (data: Ride) => {
        console.log(data);
    });
  }

  toggleChangeRide(){
    this.isRideRequestPending = !this.isRideRequestPending;
    this.isRideChanging = true;
    jQuery("#callARideModal").modal("toggle");
  }

  cancelARide(){
    let cancelRideRequest = new CancelRideRequest(true);
    this.ridesService.cancelRide(cancelRideRequest).subscribe(data => {console.log(data)});
    this.isRideRequestPending = false;
    this.isRideCancelled = true;
  }

  getAllMyRides(){
    this.ridesService.getAllMyRides().subscribe(
      data => {
        let tempRidesArr = data as Ride[];
        tempRidesArr.forEach(r => {
          let tempdate = new Date(r.timestamp);
          r.timestamp = `${tempdate.toLocaleDateString()} ${tempdate.toLocaleTimeString()}`;
          r.comment.timestamp = new Date(r.comment.timestamp);
        });
        this.ridesHistory = tempRidesArr;
      }
    )
  }

  commentCancelledRide(comment){
    console.log(comment);
    jQuery("#commentModal").modal("toggle");
    this.isRideCancelled = !this.isRideCancelled;

    let newComment = new Comment();
    newComment.description = comment.description;
    newComment.timestamp = new Date();
    this.ridesService.commentCancelledRide(newComment).subscribe(data => console.log(data));
  }

  addComment(comment: Comment){
    comment.id = comment.rideId;
    comment.timestamp = new Date();
    //comment.timestamp.
    this.ridesService.addComment(comment).subscribe(
      (data: Ride) => {
        console.log(data);
      }
    )
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
    let comment = this.ridesHistory.filter(r => r.id === rideId)[0].comment;
    comment.rating = ratingIndex + 1;
    this.ridesService.rateARide(comment).subscribe(
      (data) => {
      }
    );
  }

  filter(){

  }

  sort(){

  }

  search(){
    
  }

  useHub(input){
    console.log(input.field);
    this.notificationService.broadcastMessage(input.field);
  }
}
