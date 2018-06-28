import { User } from "./user.model";
import { Ride } from "./ride.model";

export class Comment{
  id: number;
  description: string;
  timestamp: Date;
  userId: number;
  user: User;
  rideId: number;
  ride: Ride;
  rating: number;
}