import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { ApiMessage } from '../../models/apiMessage.model';
import { LoginModel } from '../../models/login.model';

@Component({
  selector: 'driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.css']
})
export class DriverComponent implements OnInit {

  constructor(private userService: UsersService) { }

  ngOnInit() {
  }

  getUserByUsername(){
    let apiMessage = new ApiMessage(localStorage.userHash, new LoginModel(null, null, localStorage.role));
    this.userService.getUserByUsername(apiMessage).subscribe(data => {console.log(data);});
  }
}
