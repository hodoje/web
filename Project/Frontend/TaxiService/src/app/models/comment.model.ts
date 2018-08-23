import { User } from "./user.model";
import { Ride } from "./ride.model";

export class Comment{
  id: number;
  description: string;
  timestamp: Date;
  rideId: number;
  rating: number;
}