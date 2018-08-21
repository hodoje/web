import { CarsService } from './../../services/cars.service';
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
  styleUrls: ['./driver.component.scss']
})
export class DriverComponent implements OnInit {

  personalData: User;
  carData: Car;
  locationData: Location;
  shouldDisplayData = false;
  shouldDisplayCarData = false;

  constructor(private userService: UsersService, private carsService: CarsService) { }

  ngOnInit() {
  }

  getMyData(){
    if(!this.shouldDisplayData){
      //let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));

      this.userService.getUserByUsername().subscribe(
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

  getMyCarData(){
    if(!this.shouldDisplayCarData){
      //let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));

      this.carsService.getById(this.personalData.carId).subscribe(
        (data: Car) =>{
          this.carData = data;
          console.log(data);
          this.shouldDisplayCarData = !this.shouldDisplayCarData;
        },
        error => {
          this.shouldDisplayCarData = !this.shouldDisplayCarData;
        });
    }
    else{
      this.shouldDisplayCarData = !this.shouldDisplayCarData;
    }
  }

  changeMyCarData(updatedCar: Car){
    updatedCar.id = this.carData.id;
    updatedCar.driverId = this.carData.driverId;    
    this.carsService.put(updatedCar.id, updatedCar).subscribe(
      data => {
        console.log(data);
        this.shouldDisplayData = !this.shouldDisplayData;
      },
      error => {
        console.log(error);
      }
    );
  }
}
