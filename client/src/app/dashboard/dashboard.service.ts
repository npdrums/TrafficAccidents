import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as L from 'leaflet';
import { Observable, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GeoServerParams } from '../models/GeoServerParams';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = environment.apiUrl;
  private apiResource = environment.trafficAccidentsResource;
  private geoServerBaseUrl = environment.geoServerBaseUrl;
  private geoServerParams = new GeoServerParams();
  private params: HttpParams;

  private accidentTypes: string[] = [];
  private participantsCounts: string[] = [];
  private participantsStatuses: string[] = [];

  constructor(private http: HttpClient) {
    this.params = new HttpParams()
      .append('service', this.geoServerParams.service)
      .append('version', this.geoServerParams.version)
      .append('request', this.geoServerParams.request)
      .append('srsName', this.geoServerParams.srsName)
      .append('outputFormat', this.geoServerParams.outputFormat);
  }

  addFilterQueryParams(key: string, value: number) {
    const existingParams = this.params.get('viewparams');
    const paramPairs = existingParams ? existingParams.split(';') : [];
    let updatedParams = paramPairs.map(param => {
      if (param.startsWith(key + ':')) {
        if (value !== 0) {
          return `${key}:${value}`;
        } else {
          return '';
        }
      } else {
        return param;
      }
    });

    if (!paramPairs.some(param => param.startsWith(key + ':')) && value !== 0) {
      updatedParams.push(`${key}:${value}`);
    }

    updatedParams = updatedParams.filter(param => param !== '');
    this.params = this.params.set('viewparams', updatedParams.join(';'));

    if (this.params.get('viewparams') === '') {
      this.params = this.params.delete('viewparams');
    }
  }


  createTrafficAccident(trafficAccident: any) {
    return this.http.post<any>(`${this.apiUrl}${this.apiResource}`, trafficAccident);
  }

  updateTrafficAccidentDescription(id: string, description: string | null) {
    const headers = { 'Content-Type': 'application/json' };
    const content = JSON.stringify(description);
    return this.http.patch<any>(`${this.apiUrl}${this.apiResource}${id}`, content, { headers: headers });
  }

  getAccidentTypes(): Observable<string[]> {
    if (this.accidentTypes.length > 0) return of(this.accidentTypes);

    return this.http.get<string[]>(this.apiUrl + this.apiResource + 'accident-types').pipe(
      map(accidentTypes => this.accidentTypes = accidentTypes)
    );
  }

  getParticipantsCounts(): Observable<string[]> {
    if (this.participantsCounts.length > 0) return of(this.participantsCounts);

    return this.http.get<string[]>(this.apiUrl + this.apiResource + 'participants-counts').pipe(
      map(participantsCounts => this.participantsCounts = participantsCounts)
    );
  }

  getParticipantsStatuses(): Observable<string[]> {
    if (this.participantsStatuses.length > 0) return of(this.participantsStatuses);

    return this.http.get<string[]>(this.apiUrl + this.apiResource + 'participant-statuses').pipe(
      map(participantsStatuses => this.participantsStatuses = participantsStatuses)
    );
  }

  getTrafficAccident(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}${this.apiResource}${id}`);
  }

  getTrafficAccidents(bbox: L.LatLngBounds,): Observable<any> {

    let params;
    if (this.params.has('viewparams')) {
      params = this.params.append('typeNames', this.geoServerParams.typeNamesWithCategoryFilters);
    } else {
      params = this.params.append('typeNames', this.geoServerParams.typeNamesTrafficAccidents)
    }

    params = params.append('bbox', bbox.toBBoxString());

    return this.http.get<L.FeatureGroup>(this.geoServerBaseUrl, { params }).pipe(
      map(response => response)
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
      map(response => response)
    );
  }

  getSettlements(bbox: L.LatLngBounds): Observable<any> {
    const params = this.params
      .append('typeNames', this.geoServerParams.typeNamesSettlements)
      .append('bbox', bbox.toBBoxString());

    return this.http.get<L.FeatureGroup>(this.geoServerBaseUrl, { params }).pipe(
      map(response => response)
    );
  }

  deleteTrafficAccident(id: string) {
    return this.http.delete(`${this.apiUrl}${this.apiResource}${id}`);
  }
}


