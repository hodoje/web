import { RegistrationModel } from './../../models/registration.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user.model';
import { Car } from '../../models/car.model';
import { Location } from './../../models/location.model';

@Component({
  selector: 'dispatcher',
  templateUrl: './dispatcher.component.html',
  styleUrls: ['./dispatcher.component.scss']
})
export class DispatcherComponent implements OnInit {

  personalData: User;
  carData: Car;
  locationData: Location;
  shouldAddDriver = false;

  constructor(private usersService: UsersService) {
    //this.personalData = new User();
  }

  ngOnInit() {
  }

  addDriver(data){
    let postData = data as User;
    postData.role = "DRIVER";
    if(postData.nationalIdentificationNumber == ""){
      postData.nationalIdentificationNumber = null;
    }
    if(postData.phoneNumber == ""){
      postData.phoneNumber = null;
    }
    console.log(postData);
    this.usersService.post(postData).subscribe(
      data => {
        console.log(data);
      }
    );
  }

  toggleAddDriver(){
    this.shouldAddDriver = !this.shouldAddDriver;
  }
}
