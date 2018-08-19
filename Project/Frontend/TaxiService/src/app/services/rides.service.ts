import { ApiMessage } from './../models/apiMessage.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GenericService } from './generic.service';
import { RideRequest } from '../models/rideRequest';
import { ChangeRideRequest } from '../models/changeRideRequest';
import { CancelRideRequest } from '../models/cancelRideRequest';

@Injectable({
  providedIn: 'root'
})
export class RidesService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'rides', httpClient);
  }

  requestRide(rideRequestData: RideRequest){
    let apiMessage = new ApiMessage(localStorage.userHash, rideRequestData);
    return this.httpClient.post('http://localhost:3737/api/rides/rideRequest', apiMessage);
  }

  changeRide(changeRideRequestData: ChangeRideRequest){
    let apiMessage = new ApiMessage(localStorage.userHash, changeRideRequestData);
    return this.httpClient.post('http://localhost:3737/api/rides/changeRideRequest', apiMessage);
  }

  cancelRide(cancelRideRequest: CancelRideRequest){
    let apiMessage = new ApiMessage(localStorage.userHash, cancelRideRequest);
    return this.httpClient.post('http://localhost:3737/api/rides/cancelRideRequest', apiMessage);
  }  
}
