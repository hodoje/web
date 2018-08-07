import { Ride } from './../../models/ride.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ApiMessage } from '../../models/apiMessage.model';
import { LoginModel } from '../../models/login.model';
import { User } from '../../models/user.model';
import { RegistrationModel } from '../../models/registration.model';
import { RegistrationService } from '../../services/registration.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {

  personalData: User;
  ridesHistory: Ride[];
  shouldDisplayData = false;

  constructor(private userService: UsersService, private notificationService: NotificationService) {
    this.ridesHistory = new Array as Ride[];
  }

  ngOnInit() {
  }

  getMyData(){
    if(!this.shouldDisplayData){
      let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));
      this.userService.getUserByUsername(apiMessage).subscribe(
      (data: User) =>{
        this.personalData = data;
        this.shouldDisplayData = !this.shouldDisplayData;
      },
      error => {
        this.shouldDisplayData = !this.shouldDisplayData;
      });
    }
    else{
      this.shouldDisplayData = !this.shouldDisplayData;
    }
  }

  changeMyData(registrationModel: RegistrationModel){
    let updatedUser = registrationModel as User;
    updatedUser.id = this.personalData.id;
    updatedUser.isBanned = this.personalData.isBanned;
    updatedUser.role = this.personalData.role;
    this.userService.put(updatedUser.id, updatedUser).subscribe(
      data => {
        this.shouldDisplayData = !this.shouldDisplayData;
      },
      error => {
        console.log(error);
      }
    );
  }

  useHub(input){
    console.log(input.field);
    this.notificationService.broadcastMessage(input.field);
  }
}
