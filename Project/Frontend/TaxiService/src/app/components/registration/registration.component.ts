import { RegistrationModel } from './../../models/registration.model';
import { Component } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';

@Component({
  selector: 'registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent{

  genders: string[];
  constructor(private registrationService: RegistrationService) {
    this.genders = ['MALE', 'FEMALE'];
  }

  register(registrationModel: RegistrationModel){
    this.registrationService.register(registrationModel).subscribe(
      data => {
        console.log(data);
      },
      error => {
        console.log(error);
      }
    );
  }
}
