import { ApiMessage } from './../models/apiMessage.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GenericService } from './generic.service';
import { RideRequest } from '../models/rideRequest';
import { ChangeRideRequest } from '../models/changeRideRequest';
import { CancelRideRequest } from '../models/cancelRideRequest';
import { Comment } from '../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class RidesService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'rides', httpClient);
  }

  getAllMyRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllMyRides', {'headers' : headers});
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

  addComment(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/addComment', comment, {'headers' : headers});
  }

  commentCancelledRide(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/commentLatestCancelledRide', comment, {'headers' : headers});
  }

  rateARide(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/rateARide', comment, {'headers': headers})
  }
}
