import { DispatcherProcessRideRequest } from './../models/dispatcherProcessRideRequest';
import { DispatcherFormRideRequest } from './../models/dispatcherFormRideRequest';
import { RefineRidesModel } from './../models/refine.model';
import { ApiMessage } from './../models/apiMessage.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
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

  getAllRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllRides', {'headers' : headers});
  }

  getAllPendingRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllPendingRides', {'headers': headers});
  }

  getAllDispatcherRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllDispatcherRides', {'headers' : headers});
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
    return this.httpClient.post('http://localhost:3737/api/rides/addComment', comment, {'headers': headers});
  }

  commentCancelledRide(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/commentLatestCancelledRide', comment, {'headers': headers});
  }

  rateARide(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/rateARide', comment, {'headers': headers})
  }

  refine(refine){
    let headers = new HttpHeaders();
    headers = headers.append('Content-type', 'application/json');
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/refine', refine, {'headers': headers});
  }

  dispatcherRidesSearch(searchParams){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/dispatcherRidesSearch', searchParams, {'headers': headers});
  }

  formARide(dispatcherFormRideRequest: DispatcherFormRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/dispatcherFormRide', dispatcherFormRideRequest, {'headers': headers});
  }

  processARide(dispatcherProcessRideRequest: DispatcherProcessRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/dispatcherProcessRide', dispatcherProcessRideRequest, {'headers': headers});
  }
}
