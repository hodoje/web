import { Location } from './../../models/location.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ApiMessage } from '../../models/apiMessage.model';
import { LoginModel } from '../../models/login.model';
import { User } from '../../models/user.model';
import { RegistrationModel } from '../../models/registration.model';
import { Car } from '../../models/car.model';

@Component({
  selector: 'driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.css']
})
export class DriverComponent implements OnInit {

  personalData: User;
  carData: Car;
  locationData: Location;
  shouldDisplayData = false;
  shouldDisplayCarData = false;

  constructor(private userService: UsersService) { }

  ngOnInit() {
  }

  getMyData(){
    if(!this.shouldDisplayData){
      let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));

      this.userService.getUserByUsername(apiMessage).subscribe(
      (data: User) =>{
        this.personalData = data;
        console.log(data);
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
    updatedUser.carId = this.personalData.carId;
    updatedUser.driverLocationId = this.personalData.driverLocationId;
    this.userService.put(updatedUser.id, updatedUser).subscribe(
      data => {
        this.shouldDisplayData = !this.shouldDisplayData;
      },
      error => {
        console.log(error);
      }
    );
  }

  // getMyCarData(){
  //   if(!this.shouldDisplayCarData){
  //     let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));

  //     this.userService.getUserByUsername(apiMessage).subscribe(
  //       (data: User) =>{
  //         this.personalData = data;
  //         this.shouldDisplayCarData = !this.shouldDisplayCarData;
  //       },
  //       error => {
  //         this.shouldDisplayCarData = !this.shouldDisplayCarData;
  //       });

  //   }
  //   else{
  //     this.shouldDisplayCarData = !this.shouldDisplayCarData;
  //   }
  // }

  // changeMyCarData(){

  // }
}
