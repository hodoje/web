import { NavbarToLoginService } from './../../services/navbar-to-login.service';
import { Component, Output, EventEmitter } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { LoginModel } from '../../models/login.model';
import { ApiMessage } from '../../models/apiMessage.model';
import { Router } from '@angular/router';
import { LoginToNavbarService } from '../../services/login-to-navbar.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{

  isLoggedIn: boolean;
  apiRequest: ApiMessage;
  
  constructor(
    private loginService: LoginService, 
    private router: Router, 
    private loginToNavbarService: LoginToNavbarService,
    private navbarToLoginService: NavbarToLoginService
  ) { }

  ngOnInit(){
    this.navbarToLoginService.change.subscribe(
      isLoggedOut => {
        if(isLoggedOut){
          this.logout();
        }
      })
  }

  login(loginModel: LoginModel){
    this.apiRequest = new ApiMessage("", loginModel);

    this.loginService.login(this.apiRequest).subscribe(
      (data: ApiMessage) => {
        localStorage.setItem('userHash', data.key);
        localStorage.setItem('role', data.data.role);
        this.isLoggedIn = true;
        this.router.navigate(['/home']);
        this.loginToNavbarService.login();
      },
      error => {console.log(error)}
    );
  }

  logout(){
    this.apiRequest = new ApiMessage(localStorage.userHash, null);
    this.loginService.logout(this.apiRequest).subscribe(
      (data: ApiMessage) => {
        if(localStorage.userHash === data.key){
          localStorage.userHash = null;
          localStorage.role = null;
          this.isLoggedIn = false;
        }
      },
      error => {console.log(error)}
    );
  }
}
