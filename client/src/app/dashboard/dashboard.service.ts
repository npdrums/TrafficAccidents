import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as L from 'leaflet';
import { GeoServerParams } from '../models/GeoServerParams';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private geoServerBaseUrl = environment.geoServerBaseUrl;
  private geoServerParams = new GeoServerParams();
  private params: HttpParams;
  
  constructor(private http: HttpClient) {
    this.params = new HttpParams()
      .append('service', this.geoServerParams.service)
      .append('version', this.geoServerParams.version)
      .append('request', this.geoServerParams.request)
      .append('srsName', this.geoServerParams.srsName)
      .append('outputFormat', this.geoServerParams.outputFormat);
  }

  getTrafficAccidents(bbox: L.LatLngBounds): Observable<any> {
    const params = this.params
      .append('typeNames', this.geoServerParams.typeNamesTrafficAccidents)
      .append('bbox', bbox.toBBoxString());

    return this.http.get<L.FeatureGroup>(this.geoServerBaseUrl, { params }).pipe(
      map(response => {
        console.log(response);
        return response;
      })
    );
  }

  getCities(bbox: L.LatLngBounds): Observable<any> {
    const params = this.params
      .append('typeNames', this.geoServerParams.typeNamesCities)
      .append('bbox', bbox.toBBoxString());

    return this.http.get<L.FeatureGroup>(this.geoServerBaseUrl, { params }).pipe(
      map(response => {
        console.log(response);
        return response;
      })
    );
  }

  getMunicipalities(bbox: L.LatLngBounds): Observable<any> {
    const params = this.params
      .append('typeNames', this.geoServerParams.typeNamesMunicipalities)
      .append('bbox', bbox.toBBoxString());

    return this.http.get<L.FeatureGroup>(this.geoServerBaseUrl, { params }).pipe(
      map(response => {
        console.log(response);
        return response;
      })
    );
  }

  getSettlements(bbox: L.LatLngBounds): Observable<any> {
    const params = this.params
      .append('typeNames', this.geoServerParams.typeNamesSettlements)
      .append('bbox', bbox.toBBoxString());

    return this.http.get<L.FeatureGroup>(this.geoServerBaseUrl, { params }).pipe(
      map(response => {
        console.log(response);
        return response;
      })
    );
  }


}


