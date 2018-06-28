import { Injectable } from '@angular/core';
import { GenericService } from './generic.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LocationsService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'locations', httpClient);
  }
}
