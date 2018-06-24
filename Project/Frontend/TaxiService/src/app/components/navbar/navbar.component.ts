import { ApiMessage } from './../../models/apiMessage.model';
import { LoginService } from './../../services/login.service';
import { Component, OnInit } from '@angular/core';
import { LoginToNavbarService } from '../../services/login-to-navbar.service';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{
  
  isLoggedIn: boolean;

  constructor(private loginToNavbarService: LoginToNavbarService, private loginService: LoginService) { }

  ngOnInit(){
    this.loginToNavbarService.change.subscribe(
      isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
      }
    );
    if(localStorage.userHash === "null"){
      this.isLoggedIn = false;
    }
    else{
      this.isLoggedIn = true;
    }
  }

  logout(){
    let apiRequest = new ApiMessage(localStorage.userHash, null);
    this.loginService.logout(apiRequest).subscribe(
      (data: ApiMessage) => {
        if(localStorage.userHash === data.key){
          localStorage.userHash = null;
          localStorage.role = null;
          this.isLoggedIn = false;
        }
      },
      error => {
        localStorage.userHash = null;
        localStorage.role = null;
        this.isLoggedIn = false;
        console.log(error)
      }
    );
  }
}
