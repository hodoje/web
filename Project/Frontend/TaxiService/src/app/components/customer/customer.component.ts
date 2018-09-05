import { RefineRidesModel } from './../../models/refine.model';
import { Comment } from './../../models/comment.model';
import { ChangeRideRequest } from './../../models/changeRideRequest';
import { CancelRideRequest } from './../../models/cancelRideRequest';
import { RidesService } from './../../services/rides.service';
import { Ride } from './../../models/ride.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit, HostListener } from '@angular/core';
import { ApiMessage } from '../../models/apiMessage.model';
import { User } from '../../models/user.model';
import { NotificationService } from '../../services/notification.service';
import { RideStatus } from './../../models/rideStatus';
import { RideRequest } from './../../models/rideRequest';
import { FormGroup, FormControl, Validators } from '../../../../node_modules/@angular/forms';
import { RideFormValidators } from '../../common/validators/ride-form.validators';

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
  rideStatuses: string[];
  isRideRequestPending = false;
  isRideChanging = false;
  isRideCancelled = false;
  ratingList = [false, false, false, false, false];
  lastRefine: RefineRidesModel;
  pendingRide: Ride;
  successfulRideRating: number;
  latestSuccessfulRide: Ride;
  
  personalDataForm = new FormGroup({
    username: new FormControl(
      null, 
      [Validators.required, Validators.minLength(8), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    password: new FormControl(
      null, 
      [Validators.required, Validators.minLength(8), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    name: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
    ),
    lastname: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
    ),
    email: new FormControl(
      null, 
      [Validators.required, Validators.email, Validators.maxLength(254)]
    ),
    gender: new FormControl(
      null,
      Validators.required
    ),
    nationalIdentificationNumber: new FormControl(
      null, 
      [Validators.minLength(13), Validators.maxLength(13),Validators.pattern('[0-9]*')]
    ),
    phoneNumber: new FormControl(
      null, 
      [Validators.minLength(5), Validators.maxLength(10), Validators.pattern('[0-9]*')]
    )
  });

  rideForm = new FormGroup({
    location: new FormGroup({
      address: new FormGroup({
        streetName: new FormControl(
          null,
          [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
        ),
        streetNumber: new FormControl(
          null,
          [Validators.required, Validators.minLength(1), Validators.maxLength(4), Validators.pattern('[0-9]*')]
        ),
        city: new FormControl(
          null,
          [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
        ),
        postalCode: new FormControl(
          null,
          [Validators.required, Validators.minLength(1), Validators.maxLength(10), Validators.pattern('[a-zA-Z0-9]*')]
        )
      }),
      longitude: new FormControl(
        null,
        [Validators.required, RideFormValidators.checklongitudeInterval]
      ),
      latitude: new FormControl(
        null,
        [Validators.required, RideFormValidators.checklatitudeInterval]
      )
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

  successfulRideForm = new FormGroup({    
    commentDescription: new FormControl()
  });

  constructor(private usersService: UsersService, private notificationService: NotificationService, private ridesService: RidesService) {
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
    this.pendingRide = new Ride();
    let rideStatusEnumKeys = Object.keys(RideStatus);
    for(var s of rideStatusEnumKeys){
      this.rideStatuses.push(s);
    }
    this.lastRefine = new RefineRidesModel();
    this.getMyData();
    this.getAllMyRides();
  }

  get pdForm(){
    return this.personalDataForm.controls;
  }

  get rdForm(){
    return this.rideForm.controls;
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

  // Only for xx.yy.zzzz aa:bb:cc type of dates
  private parseToJsonDateString(unparsedDate: string){
    let year = unparsedDate.substr(6, 4);
    let month = unparsedDate.substr(0, 2);
    let day = unparsedDate.substr(3,2);
    let time = unparsedDate.substring(13);
    return `${month}/${day}/${year} ${time}`;
  }

  private checkIfTwoMinutesPassed(timestamp: string){
    if(timestamp.indexOf('/') === -1){
      timestamp = this.parseToJsonDateString(timestamp);
    }
    let past = new Date(timestamp).getTime();
    let twoMins = 1000 * 60 * 2;
    let now = Date.now();
    return (now - past < twoMins) ? false : true;
  }

  rateSuccessfulRideRating(starIndex){
    this.successfulRideRating = starIndex + 1;
  }

  commentSuccessfulRide(){
    let comment = new Comment();
    comment.rideId = this.latestSuccessfulRide.id;
    comment.rating = this.successfulRideRating;
    comment.description = this.successfulRideForm.value.commentDescription;
    this.ridesService.addComment(comment).subscribe(
      (data: Ride) => {
        var rideIndex = this.ridesHistory.findIndex(r => r.id === data.id);
        this.ridesHistory[rideIndex] = this.parseSingleRide(data);
        this.latestSuccessfulRide = null;
        this.successfulRideRating = 0;
        jQuery('#successfulRideModal').modal('toggle');
    });
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
      this.personalDataForm.markAsPristine();
    });
  }

  getAllMyRides(){
    this.ridesService.getAllMyRides().subscribe(
      (data: Ride[]) => {
        this.ridesHistory = this.parseRides(data);

        let createdRide = this.ridesHistory.find(r => r.rideStatus === RideStatus.CREATED ||
                                                      r.rideStatus === RideStatus.ACCEPTED || 
                                                      r.rideStatus === RideStatus.PROCESSED);
        if(createdRide !== undefined){
          this.rideForm.patchValue({
            location: createdRide.startLocation,
            carType: createdRide.carType
          });
          this.isRideRequestPending = true;
          this.pendingRide = createdRide;
        }
        else{
          let successfulRides = this.ridesHistory.filter(r => r.rideStatus === RideStatus.SUCCESSFUL);
          let sortedRides = successfulRides.sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());
          this.latestSuccessfulRide = sortedRides.pop();
          if(this.latestSuccessfulRide !== undefined){
            if(!this.checkIfTwoMinutesPassed(this.latestSuccessfulRide.timestamp)){
              jQuery('#successfulRideModal').modal('toggle');
            }
          }
        }
      }
    )
  }

  changeMyData(){
    let updatedUser = new User();
    updatedUser = this.personalDataForm.value;
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

  callARide(rideRequest: RideRequest){
    jQuery("#callARideModal").modal("toggle");

    if(rideRequest.carType === null){
      rideRequest.carType = 'DEFAULT';
    }
    this.ridesService.requestRide(rideRequest).subscribe(
      (data: Ride) =>{
        this.pendingRide = this.parseSingleRide(data);
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
    let rideToComment = this.ridesHistory.find(r => r.id === rideId);
    if(rideToComment.comments.length === 0){
      let comment = new Comment();
      comment.rating = ratingIndex + 1;
      comment.rideId = rideId;
      comment.userId = this.personalData.id;
      this.ridesService.addComment(comment).subscribe(
        (data: Ride) => {
          var rideIndex = this.ridesHistory.findIndex(r => r.id === data.id);
        this.ridesHistory[rideIndex] = this.parseSingleRide(data);
        }
      );
    }
    else{
      let comment = rideToComment.comments.find(c => c.userId === this.personalData.id);  
      comment.rating = ratingIndex + 1;
      this.ridesService.rateARide(comment).subscribe();  
    }
  }

  refineRides(typeOfButton){
    if(typeOfButton == "FILTER"){
      this.refineForm.value.search = null;
      this.refineForm.controls.search.reset();
    }
    else if(typeOfButton == "SEARCH"){
      this.refineForm.value.filter = null;
      this.refineForm.controls.filter.reset();
    }
    else if(typeOfButton == "SORT"){
      if(this.lastRefine.filter !== null){
        this.refineForm.value.search = null;
        this.refineForm.controls.search.reset();
      }
      else{
        this.refineForm.value.filter = null;
        this.refineForm.controls.filter.reset();
      }
    }
    this.lastRefine = this.refineForm.value;
    this.ridesService.refine(this.refineForm.value).subscribe(
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
