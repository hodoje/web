import { RefineRidesModel } from './../../models/refine.model';
import { Comment } from './../../models/comment.model';
import { ChangeRideRequest } from './../../models/changeRideRequest';
import { CancelRideRequest } from './../../models/cancelRideRequest';
import { RidesService } from './../../services/rides.service';
import { Location } from './../../models/location.model';
import { Ride } from './../../models/ride.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit, HostListener } from '@angular/core';
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
  lastRefine: RefineRidesModel;

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

  constructor(private userService: UsersService, private notificationService: NotificationService, private ridesService: RidesService) {
    // jQuery('body').on('hidden.bs.modal', '#callARideModal', function(this){
    //   if(!this.isRideRequestPending){
    //     this.isRideRequestPending = true;
    //     //this.isRideChanging = false;
    //     //jQuery("#callARideModal").modal("toggle");
    //   }
    // }.bind(this));
  }

  ngOnInit() {
    this.personalData = new User();
    this.ridesHistory = [];
    this.rideStatuses = [];
    this.lastRefine = new RefineRidesModel();
    let rideStatusEnumKeys = Object.keys(RideStatus);
    for(var s of rideStatusEnumKeys){
      this.rideStatuses.push(s);
    }
    this.getMyData();
    this.getAllMyRides();
  }

  getMyData(){
    this.userService.getUserByUsername().subscribe(
    (data: User) =>{
      this.personalData = data;
      this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
    });
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

  getAllMyRides(){
    this.ridesService.getAllMyRides().subscribe(
      (data: Ride[]) => {
        this.ridesHistory = this.parseRides(data);
        this.ridesHistory.forEach(r => {
          r.comments = this.setCurrentUserCommentToBottom(r.comments);
        });

        let createdRide = this.ridesHistory.find(r => r.rideStatus === 'CREATED');
        if(createdRide !== undefined){
          this.rideForm = new FormGroup({
            location: new FormGroup({
              address: new FormGroup({
                streetName: new FormControl(createdRide.startLocation.address.streetName),
                streetNumber: new FormControl(createdRide.startLocation.address.streetNumber),
                city: new FormControl(createdRide.startLocation.address.city),
                postalCode: new FormControl(createdRide.startLocation.address.postalCode)
              }),
              longitude: new FormControl(createdRide.startLocation.longitude),
              latitude: new FormControl(createdRide.startLocation.latitude)
            }),
            carType: new FormControl(createdRide.carType)
          });
          this.isRideRequestPending = true;
        }
      }
    )
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
        this.ridesHistory.push(this.parseSingleRide(data));
    });
    this.isRideRequestPending = true;
  }

  changeARide(changeRideRequest: ChangeRideRequest){
    jQuery("#callARideModal").modal("toggle");
    this.isRideChanging = false;

    if(changeRideRequest.carType === null){
      changeRideRequest.carType = 'DEFAULT';
    }

    this.ridesService.changeRide(changeRideRequest).subscribe(
      (data: Ride) => {
        var rideIndex = this.ridesHistory.findIndex(r => r.id === data.id);
        this.ridesHistory[rideIndex] = this.parseSingleRide(data);
      }
    );
  }

  toggleChangeRide(){
    this.isRideChanging = true;
  }

  cancelARide(){
    let cancelRideRequest = new CancelRideRequest(true);
    this.ridesService.cancelRide(cancelRideRequest).subscribe(
      (data: Ride) => {
        var rideIndex = this.ridesHistory.findIndex(r => r.id === data.id);
        this.ridesHistory[rideIndex] = this.parseSingleRide(data);
      }
    );
    this.isRideRequestPending = false;
    this.isRideCancelled = true;
    this.rideForm.reset();
  }

  commentCancelledRide(comment: Comment){
    jQuery("#commentModal").modal("toggle");
    this.isRideCancelled = !this.isRideCancelled;
    this.ridesService.commentCancelledRide(comment).subscribe(
      (data: Ride) => {
        var rideIndex = this.ridesHistory.findIndex(r => r.id === data.id);
        this.ridesHistory[rideIndex] = this.parseSingleRide(data);
      }
    );
  }

  addComment(comment: Comment){
    comment.id = comment.rideId;
    this.ridesService.addComment(comment).subscribe(
      (data: Ride) => {
        var rideIndex = this.ridesHistory.findIndex(r => r.id === data.id);
        this.ridesHistory[rideIndex] = this.parseSingleRide(data);
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
    let comment = this.ridesHistory.filter(r => r.id === rideId)[0].comments.filter(c => c.userId === this.personalData.id)[0];
    comment.rating = ratingIndex + 1;
    this.ridesService.rateARide(comment).subscribe();
  }

  refineRides(refineForm, typeOfButton){
    if(typeOfButton == "FILTER"){
      refineForm.value.search = null;
      refineForm.controls.search.reset();
    }
    else if(typeOfButton == "SEARCH"){
      refineForm.value.filter = null;
      refineForm.controls.filter.reset();
    }
    else if(typeOfButton == "SORT"){
      if(this.lastRefine.filter !== null){
        refineForm.value.search = null;
        refineForm.controls.search.reset();
      }
      else{
        refineForm.value.filter = null;
        refineForm.controls.filter.reset();
      }
    }
    this.lastRefine = refineForm.value;
    this.ridesService.refine(refineForm.value).subscribe(
      (data: Ride[]) =>{
        this.ridesHistory = this.parseRides(data);
      },
      error => {
        console.log(error);
      }
    );
  }

  useHub(input){
    this.notificationService.broadcastMessage(input.field);
  }
}
