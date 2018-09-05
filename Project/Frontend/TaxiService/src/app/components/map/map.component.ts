import { MapsService } from './../../services/maps.service';
import { MapInfo } from '../../models/map-information.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {

  mapInfo: MapInfo;

  constructor(private mapsService: MapsService) {
    this.mapInfo = new MapInfo(45.240683289192, 19.844917724888887, 
    "assets/ftn.png",
    "Jugodrvo" , "" , "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");
  }

  ngOnInit() {
  }

  placeMarker(event){
    this.mapInfo.long = event.coords.lng;
    this.mapInfo.lat = event.coords.lat;
    console.log(this.mapInfo);
  }

}
