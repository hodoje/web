import { NavbarToLoginService } from './../../services/navbar-to-login.service';
import { Component, OnInit } from '@angular/core';
import { LoginToNavbarService } from '../../services/login-to-navbar.service';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{
  
  isLoggedIn: boolean;

  constructor(private loginToNavbarService: LoginToNavbarService, private navbarToLoginService: NavbarToLoginService) { }

  ngOnInit(){
    this.loginToNavbarService.change.subscribe(
      isLoggedIn => {
        if(isLoggedIn){
          this.isLoggedIn = isLoggedIn;
        }
      }
    );
  }

  logout(){
    this.isLoggedIn = !this.isLoggedIn;
    this.navbarToLoginService.logout();
  }
}
