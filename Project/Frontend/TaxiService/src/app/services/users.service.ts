import { ApiMessage } from './../models/apiMessage.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GenericService } from './generic.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'users', httpClient);
  }

  getUserByUsername(apiMessage: ApiMessage){
    return this.httpClient.post('http://localhost:3737/api/users/getuserbyusername', apiMessage);
  }
}
