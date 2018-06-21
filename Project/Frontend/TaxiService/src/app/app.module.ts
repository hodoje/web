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

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SidebarComponent,
    ContentComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    LoginService,
    RegistrationService,
    UsersService,
    RidesService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
