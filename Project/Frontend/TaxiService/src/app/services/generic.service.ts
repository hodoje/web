import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

// @Injectable({
//   providedIn: 'root'
// })
export abstract class GenericService {

  constructor(private url: string, private endpoint: string, protected httpClient: HttpClient) { }

  public getById(id: number){
    return this.httpClient.get(`${this.url}/${this.endpoint}/${id}`);
  }

  public getAll(){
    return this.httpClient.get(`${this.url}/${this.endpoint}`);
  }

  public post(item: any){
    return this.httpClient.post(`${this.url}/${this.endpoint}`, item);
  }

  public put(id: number, item: any){
    return this.httpClient.put(`${this.url}/${this.endpoint}/${id}`, item);
  }

  public delete(id: number){
    return this.httpClient.delete(`${this.url}/${this.endpoint}/${id}`);
  }
}
