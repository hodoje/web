import { Comment } from './comment.model';
import { User } from './user.model';
import { Location } from './location.model';
export class Ride{
  id: number;
  timestamp: string;
  startLocation: Location;
  carType: string;
  customer: User;
  destinationLocation: Location;
  dispatcher: User;
  driver: User;
  price: number;
  comment: Comment;
  rideStatus: string;
}