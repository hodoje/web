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
  ridesHistory: Ride[];

  rideStatuses: string[];
  isRideRequestPending = false;
  isRideChanging = false;
  isRideCancelled = false;
  ratingList = [false, false, false, false, false];
  // lastRefine: RefineRidesModel;
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

  constructor(private userService: UsersService, private ridesService: RidesService) { }

  ngOnInit() {
    this.personalData = new User();
    this.ridesHistory = [];
    this.rideStatuses = [];
    this.getMyData();
  }

  getMyData(){
    this.userService.getUserByUsername().subscribe(
    (data: User) =>{
      this.personalData = data;
    });
  }

  // changeMyData(registrationModel: RegistrationModel){
  //   let updatedUser = registrationModel as User;
  //   updatedUser.id = this.personalData.id;
  //   updatedUser.isBanned = this.personalData.isBanned;
  //   updatedUser.role = this.personalData.role;

  //   let apiMessage = new ApiMessage(localStorage.userHash, updatedUser);

  //   this.userService.put(updatedUser.id, apiMessage).subscribe(
  //     (data: string) => {
  //       localStorage.userHash = data;
  //       this.shouldDisplaySaveChanges = !this.shouldDisplaySaveChanges;
  //     },
  //     error => {
  //       console.log(error);
  //     }
  //   );
  // }

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

  // getAllMyRides(){
  //   this.ridesService.getAllMyRides().subscribe(
  //     (data: Ride[]) => {
  //       this.ridesHistory = this.parseRides(data);
  //     }
  //   )
  // }

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
