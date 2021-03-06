import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { UsersComponent } from './components/users/users.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ContentComponent } from './components/content/content.component';
import { CustomerComponent } from './components/customer/customer.component';
import { DriverComponent } from './components/driver/driver.component';
import { DispatcherComponent } from './components/dispatcher/dispatcher.component';
import { RegistrationComponent } from './components/registration/registration.component';

import { LocationsService } from './services/locations.service';
import { CarsService } from './services/cars.service';
import { RidesService } from './services/rides.service';
import { AccessService } from './services/access.service';
import { LoginToNavbarService } from './services/login-to-navbar.service';
import { RegistrationService } from './services/registration.service';
import { UsersService } from './services/users.service';
import { NotificationService } from './services/notification.service';

import { ContentGuard } from './guards/content.guard';
import { LoginGuard } from './guards/login.guard';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterModule, Router, CanActivate } from '@angular/router';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AgmCoreModule } from '@agm/core';

const Routes = [
  {
    path: "home",
    component: HomeComponent
  },
  {
    path: "profile",
    component: ContentComponent,
    canActivate: [ContentGuard]
  },
  {
    path: "login",
    component: LoginComponent,
    canActivate: [LoginGuard]
  },
  {
    path: "register",
    component: RegistrationComponent,
    canActivate: [LoginGuard]
  },
  {
    path: "**",
    redirectTo: "home"
  }
]

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    UsersComponent,
    HomeComponent,
    RegistrationComponent,
    LoginComponent,
    ContentComponent,
    CustomerComponent,
    DriverComponent,
    DispatcherComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(Routes),
    MDBBootstrapModule.forRoot(),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyBrjeABCsOwW1rhi7eW6_b_fWF8OK2HeiA'
    })
  ],
  providers: [
    AccessService,
    RegistrationService,
    UsersService,
    RidesService,
    CarsService,
    LocationsService,
    LoginToNavbarService,
    NotificationService,
    LoginGuard,
    ContentGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }