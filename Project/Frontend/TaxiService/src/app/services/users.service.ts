import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { stringify } from 'querystring';
import { JsonPipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private url = 'http://localhost:3737/api/users/';
  data: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getAll();
   }

  getAll(){
    this.http.get(this.url).subscribe((data:Object) => {
      this.data = data;
    });
    return this.data;
  }

  getById(id: number){
    this.http.get(this.url + id).subscribe((data: Object) => {
      this.data = data;
    })
    return this.data;
  }

}
