import { Address } from './address.model';
export class Location{
  id: number;
  address: Address;
  longitude: number;
  latitude: number;

  constructor(){
    this.address = new Address();
  }
}