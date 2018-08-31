import { User } from "./user.model";

export class Comment{
  id: number;
  description: string;
  timestamp: Date;
  rideId: number;
  rating: number;
  user: User;
  userId: number;
}