import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { LoginModel } from '../../models/login.model';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{

  constructor(private loginService: LoginService) { }

  login(loginModel: LoginModel){
    this.loginService.login(loginModel).subscribe(
      data => {console.log(data)},
      error => {console.log(error)}
    );
  }

  logout(loginModel: LoginModel){
    this.loginService.logout(loginModel).subscribe(
      data => {console.log(data)},
      error => {console.log(error)}
    );;
  }
}
