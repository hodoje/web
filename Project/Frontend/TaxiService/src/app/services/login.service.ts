import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from '../models/login.model';

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

  login(loginModel: LoginModel){
    return this.httpClient.post(`${this.url}/${this.endpoint}/login`, loginModel);
  }

  logout(loginModel: LoginModel){
    return this.httpClient.post(`${this.url}/${this.endpoint}/logout`, loginModel);
  }
}
