import { RoleGuard } from './guards/role.guard';
import { LoginToNavbarService } from './services/login-to-navbar.service';
import { GenericService } from './services/generic.service';
import { RegistrationService } from './services/registration.service';
import { UsersService } from './services/users.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Component } from '@angular/core';
import { RouterModule, Router, CanActivate } from '@angular/router';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { RidesService } from './services/rides.service';
import { LoginService } from './services/login.service';
import { UsersComponent } from './components/users/users.component';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { FormsModule } from '@angular/forms';
import { ContentComponent } from './components/content/content.component';
import { CustomerComponent } from './components/customer/customer.component';
import { DriverComponent } from './components/driver/driver.component';
import { DispatcherComponent } from './components/dispatcher/dispatcher.component';
import { LoginGuard } from './guards/login.guard';

const ContentChildRoutes = [
  {
    path: "customer",
    component: CustomerComponent
  },
  {
    path: "driver",
    component: DriverComponent
  },
  {
    path: "dispatcher",
    component: DispatcherComponent
  }
]

const Routes = [
  {
    path: "home",
    component: HomeComponent
  },
  {
    path: "content",
    component: ContentComponent,
    children: ContentChildRoutes
  },
  {
    path: "login",
    component: LoginComponent,
    canActivate: [LoginGuard]
  },
  {
    path: "register",
    component: RegistrationComponent
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
    SidebarComponent,
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
    RouterModule.forRoot(Routes)
  ],
  providers: [
    GenericService,
    LoginService,
    RegistrationService,
    UsersService,
    RidesService,
    LoginToNavbarService,
    LoginGuard,
    RoleGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }