import { Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {
  private map!: L.Map;

  constructor() {}

  ngOnInit(): void {
    this.initMap();
  }

  private initMap() {
    this.map = L.map('map', {
      center: [44.0165, 21.0059],
      zoom: 7,
    });

    const tiles = L.tileLayer(environment.openStreetMapsTilesUrl, {
      maxZoom: 18,
      minZoom: 3,
      attribution:
        '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
    });

    tiles.addTo(this.map);
  }

}
