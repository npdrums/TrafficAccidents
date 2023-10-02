import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { LatLngBounds } from 'leaflet';
import { FeatureCollection } from 'geojson';
import { faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent implements OnInit {
  accidentTypes: string[] = [];
  participantsNominalCounts: string[] = [];
  participantsStatuses: string[] = [];
  trafficAccidents: FeatureCollection | null | undefined;
  cities: FeatureCollection | null | undefined;
  municipalities: FeatureCollection | null | undefined;
  settlements: FeatureCollection | null | undefined;

  municipalitiesToggleState = false;
  citiesToggleState = false;
  settlementsToggleState = false;

  zoom!: number;
  private bbox!: LatLngBounds;

  faTriangleExclamation = faTriangleExclamation

  constructor(private service: DashboardService,
    private cd: ChangeDetectorRef,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getAccidentTypes();
    this.getParticipantsCounts();
    this.getParticipantsStatuses();
  }

  getAccidentTypes() {
    this.service.getAccidentTypes().subscribe({
      next: response => {
        this.accidentTypes = ["All", ...response]
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    })
  }

  getParticipantsCounts() {
    this.service.getParticipantsCounts().subscribe({
      next: response => {
        this.participantsNominalCounts = ["All", ...response]
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    })
  }

  getParticipantsStatuses() {
    this.service.getParticipantsStatuses().subscribe({
      next: response => {
        this.participantsStatuses = ["All", ...response]
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    })
  }

  formatEnumResponse(text: string) {
    const regex = /([A-Z])(?=[a-z])/g;
    return text.replace(regex, " $1")
  }

  onAccidentTypeSelected(event: any) {
    let index: number = event.target["selectedIndex"];
    this.service.addFilterQueryParams('at', index);

    if (this.zoom > 15) {
      this.getTrafficAccidents(this.bbox);
    }
  }

  onParticipantsNominalCountSelected(event: any) {
    let index: number = event.target["selectedIndex"];
    this.service.addFilterQueryParams('pnc', index);

    if (this.zoom > 15) {
      this.getTrafficAccidents(this.bbox);
    }
  }

  onParticipantsStatusSelected(event: any) {
    let index: number = event.target["selectedIndex"];
    this.service.addFilterQueryParams('ps', index);

    if (this.zoom > 15) {
      this.getTrafficAccidents(this.bbox);
    }
  }

  onMapChanged(values: { bbox: LatLngBounds, zoom: number }) {
    this.bbox = values.bbox;
    this.zoom = values.zoom;

    if (this.zoom > 15) {
      this.getTrafficAccidents(this.bbox);
    }

    if (this.citiesToggleState && this.zoom >= 10) {
      this.getCities(this.bbox);
    }

    if (this.municipalitiesToggleState && this.zoom >= 10) {
      this.getMunicipalities(this.bbox);
    }

    if (this.settlementsToggleState && this.zoom >= 10) {
      this.getSettlements(this.bbox);
    }

    this.cd.detectChanges();
  }

  municipalitiesModelChange(event: any) {
    if (!event) {
      this.municipalities = null
      this.cd.detectChanges();
    }
  }

  settlementsModelChange(event: any) {
    if (!event) {
      this.settlements = null
      this.cd.detectChanges();
    }
  }

  citiesModelChange(event: any) {
    if (!event) {
      this.settlements = null
      this.cd.detectChanges();
    }
  }

  getTrafficAccidents(bbox: LatLngBounds) {
    this.service.getTrafficAccidents(bbox).subscribe({
      next: response => {
        this.trafficAccidents = response
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    });
  }

  getCities(bbox: LatLngBounds) {
    this.service.getCities(bbox).subscribe({
      next: response => {
        this.cities = response
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    });
  }

  getMunicipalities(bbox: LatLngBounds) {
    this.service.getMunicipalities(bbox).subscribe({
      next: response => {
        this.municipalities = response
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    });
  }

  getSettlements(bbox: LatLngBounds) {
    this.service.getSettlements(bbox).subscribe({
      next: response => this.settlements = response,
      error: error => this.toastr.error(error.message, "Ooops!")
    });
  }
}
