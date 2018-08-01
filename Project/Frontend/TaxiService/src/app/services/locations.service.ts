import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GenericService } from './generic.service';

@Injectable({
  providedIn: 'root'
})
export class LocationsService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'locations', httpClient);
  }
}