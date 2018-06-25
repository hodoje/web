import { User } from "./user.model";
import { Ride } from "./ride.model";

export class Comment{
  id: number;
  description: string;
  timestamp: Date;
  user: User;
  ride: Ride;
  rating: number;
}