import { Comment } from './comment.model';
import { User } from './user.model';
import { RideStatus } from "./rideStatus";

export class ChangeRideRequest{
  id: number;
  timestamp: Date;
  startLocationId: number;
  startLocation: Location;
  destinationLocationId: number;
  destinationLocation: Location;
  price: number;
  rideStatus: string;
  carType: string;
  customerId: number;
  customer: User;
  dispatcherId: number;
  dispatcher: User;
  driverId: number;
  driver: User;
  commentId: number;
  comment: Comment;
}