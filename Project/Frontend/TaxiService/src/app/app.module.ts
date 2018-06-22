import { GenericService } from './services/generic.service';
import { RegistrationService } from './services/registration.service';
import { UsersService } from './services/users.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { ContentComponent } from './components/content/content.component';
import { RidesService } from './services/rides.service';
import { LoginService } from './services/login.service';
import { UsersComponent } from './components/users/users.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SidebarComponent,
    ContentComponent,
    UsersComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [
    GenericService,
    LoginService,
    RegistrationService,
    UsersService,
    RidesService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
