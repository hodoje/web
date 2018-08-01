import { Location } from './location.model';
import { Car } from "./car.model";

export class User{
  id: number;
  username: string;
  password: string;
  name: string;
  lastname: string;
  gender: string;
  nationalIdentificationNumber: string;
  phoneNumber: string;
  email: string;
  isBanned: boolean;
  role: string;
  carId: number;
  car: Car;
  driverLocationId: number;
  driverLocation: Location;
}