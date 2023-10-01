import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FeatureCollection } from 'geojson';
import * as L from 'leaflet';
import { Subject, debounceTime } from 'rxjs';
import { MapInit } from 'src/app/models/mapInit';
import { environment } from 'src/environments/environment';
import { DashboardService } from '../dashboard.service';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MapComponent implements OnInit, OnChanges {
  @Input() trafficAccidents: FeatureCollection | null | undefined;
  @Input() cities: FeatureCollection | null | undefined;
  @Input() municipalities: FeatureCollection | null | undefined;
  @Input() settlements: FeatureCollection | null | undefined;
  @Output() mapChange = new EventEmitter<{ bbox: L.LatLngBounds, zoom: number }>();

  private map!: L.Map;
  private mapInit: MapInit;
  private featureGroup!: L.FeatureGroup;

  private citiesLayers: L.Layer[] = [];
  private municipalitiesLayers: L.Layer[] = [];
  private settlementsLayers: L.Layer[] = [];
  private trafficAccidentsLayers: L.Layer[] = [];

  private zoomChange$ = new Subject<number>();

  constructor(private service: DashboardService,
    private router: Router,
    private cd: ChangeDetectorRef,
    private toastr: ToastrService) {
    this.mapInit = new MapInit();
  }

  ngOnInit(): void {
    this.initMap();
    this.bindEvents();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['cities']) {
      this.showCities();
    }

    if (changes['municipalities']) {
      this.showMunicipalities();
    }

    if (changes['settlements']) {
      this.showSettlements();
    }

    if (changes['trafficAccidents']) {
      this.showTrafficAccidents();
    }
  }

  private bindEvents() {
    this.map.on('moveend', () => {
      this.emitMapChange(this.map.getBounds(), this.map.getZoom());
    });

    this.map.on('zoomend', () => {
      this.emitMapChange(this.map.getBounds(), this.map.getZoom());
    });

    this.map.on('zoomend', () => {
      this.zoomChange$.next(this.map.getZoom());
    });

    this.zoomChange$.pipe(debounceTime(100)).subscribe((zoom) => {
      if (zoom < 10) {
        this.featureGroup.clearLayers();
      }
    })

    this.map.on('contextmenu', (event) => {
      const lat = event.latlng.lat;
      const lng = event.latlng.lng;
      this.handleCreateTrafficAccident(lat, lng);
    })
  }

  private initMap() {
    this.map = L.map('map', {
      center: [this.mapInit.latitude, this.mapInit.longitude],
      zoom: this.mapInit.defaultZoom,
    });

    const tiles = L.tileLayer(environment.openStreetMapsTilesUrl, {
      maxZoom: this.mapInit.maxZoom,
      minZoom: this.mapInit.minZoom,
      attribution: this.mapInit.attribution,
    });

    tiles.addTo(this.map);

    this.featureGroup = L.featureGroup().addTo(this.map);
  }

  private createPopUp(data: any) {
    return `<div class="m-1">` +
      `<div id="traffic-accident-id" hidden>${data.traffic_accident_id}</div>` +
      `<div><span class="fw-bold">Case No.:</span>${data.case_number}</span></div>` +
      `<div><span class="fw-bold">Police Department:</span> ${data.police_department}</span></div>` +
      `<div><span class="fw-bold">Description (Serbian):</span> ${data.description}</div>` +
      `<button id="edit-ta" class="btn btn-outline-primary btn-sm my-1 me-2">Edit</button>` +
      `<button id="delete-ta" class="btn btn-danger btn-sm my-1">Delete</button></div>`;
  }

  private handleCreateTrafficAccident(lat: number, lng: number) {
    const navigationExtras: NavigationExtras = { state: { lat: lat, lng: lng } };
    this.router.navigateByUrl('/traffic-accident', navigationExtras);
  }

  private handleEditButtonClick(id: string) {
    this.router.navigate(['/traffic-accident', id])
  }

  private handleDeleteButtonClick(id: string) {
    this.service.deleteTrafficAccident(id).subscribe({
      next: _ => {
        this.map.setZoom(this.map.getZoom())
        this.toastr.success("Deleted a traffic accident!");
      },
      error: error => this.toastr.error(error, "Ooops!")
    });
  }
  
  private addPopUpEventListeners() {
    this.map.off('popupopen');
    this.map.on('popupopen', (event: L.PopupEvent) => {
      const popupElement = event.popup.getElement();
      const trafficAccidentId = popupElement?.querySelector('#traffic-accident-id');
      const editBtn = popupElement?.querySelector('#edit-ta');
      const deleteBtn = popupElement?.querySelector('#delete-ta');

      if (editBtn) {
        editBtn.addEventListener('click', () => {
          const id = trafficAccidentId?.textContent;
          this.handleEditButtonClick(id!);
          this.map.closePopup(event.popup);
        })
      }

      if (deleteBtn) {
        deleteBtn.addEventListener('click', () => {
          const id = trafficAccidentId?.textContent;
          this.handleDeleteButtonClick(id!);
          this.map.closePopup(event.popup);
        })
      }
    });
  }

  private showTrafficAccidents() {
    if (this.trafficAccidents) {
      this.clearLayers(this.trafficAccidentsLayers);
      let featureGroup = L.featureGroup();
      this.trafficAccidents.features.forEach((feature => {
        if (feature.geometry.type === 'Point') {
          const lon = feature.geometry.coordinates[0];
          const lat = feature.geometry.coordinates[1];
          const circle = L.circleMarker([lat, lon], { radius: 5 });
          circle.bindPopup(this.createPopUp(feature.properties))
          circle.addTo(featureGroup);
        }
      }))

      this.featureGroup.addLayer(featureGroup);
      this.trafficAccidentsLayers.push(featureGroup);
      this.addPopUpEventListeners();
    }
  }

  private showMunicipalities() {
    if (this.municipalities) {
      this.clearLayers(this.municipalitiesLayers);
      const layer = L.geoJson(this.municipalities?.features, {
        style: _ => ({
          weight: 3,
          opacity: 0.7,
          color: '#dF0000',
          fillOpacity: 0,
        })
      });

      this.featureGroup.addLayer(layer);
      this.municipalitiesLayers.push(layer);
    }
  }

  private showCities() {
    if (this.cities) {
      this.clearLayers(this.citiesLayers);
      const layer = L.geoJson(this.cities?.features, {
        style: _ => ({
          weight: 3,
          opacity: 0.7,
          color: '#009008',
          fillOpacity: 0,
        })
      });

      this.featureGroup.addLayer(layer);
      this.citiesLayers.push(layer);
    }
  }

  private showSettlements() {
    if (this.settlements) {
      this.clearLayers(this.settlementsLayers);
      const layer = L.geoJson(this.settlements?.features, {
        style: _ => ({
          weight: 3,
          opacity: 0.7,
          color: '#800080',
          fillOpacity: 0,
        })
      });

      this.featureGroup.addLayer(layer);
      this.settlementsLayers.push(layer);
    }
  }

  private clearLayers(layers: L.Layer[]) {
    layers.forEach(layer => this.featureGroup.removeLayer(layer));
    layers.length = 0;
  }

  emitMapChange(bbox: L.LatLngBounds, zoom: number) {
    this.mapChange.emit({ bbox, zoom });
  }
}
