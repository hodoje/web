import { ApiMessage } from './../models/apiMessage.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService{

  url: string;
  endpoint: string;
  constructor(private httpClient: HttpClient) {
    this.url = 'http://localhost:3737/api';
    this.endpoint = 'access';
  }

  login(loginRequest: ApiMessage){
    return this.httpClient.post(`${this.url}/${this.endpoint}/login`, loginRequest);
  }

  logout(logoutRequest: ApiMessage){
    return this.httpClient.post(`${this.url}/${this.endpoint}/logout`, logoutRequest);
  }
}
