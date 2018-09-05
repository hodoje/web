//import { google } from '@agm/core/services/google-maps-types';
import { MapInfo } from '../../models/map-information.model';
import { Component, OnInit } from '@angular/core';

declare var google;

@Component({
  selector: 'map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {

  mapInfo: MapInfo;

  constructor() {
    this.mapInfo = new MapInfo(45.240683289192, 19.844917724888887, 
    "Jugodrvo" , "");
  }

  ngOnInit() {
  }

  placeMarker(event){
    this.mapInfo.lat = event.coords.lat;
    this.mapInfo.long = event.coords.lng;

    let resultLocationData: any;
    let geocoder = new google.maps.Geocoder();
    let mylatLng = new google.maps.LatLng(this.mapInfo.lat, this.mapInfo.long);
    let geocoderRequest = {latLng: mylatLng};
    geocoder.geocode(geocoderRequest, function(result, status){
      resultLocationData = result[0];
      console.log(result[0]);
    });
  }
}
