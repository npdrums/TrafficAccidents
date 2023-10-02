import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { DashboardService } from '../dashboard.service';
import { ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Navigation, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-traffic-accident-form',
  templateUrl: './traffic-accident-form.component.html',
  styleUrls: ['./traffic-accident-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TrafficAccidentFormComponent implements OnInit {
  accidentTypes: string[] = [];
  participantsNominalCounts: string[] = [];
  participantsStatuses: string[] = [];
  private navigation: Navigation | null = this.router.getCurrentNavigation();

  trafficAccidentId!: string | null;
  isEdit: boolean = false;

  form = this.fb.group({
    caseNumber: [null, Validators.required],
    policeDepartment: [null, Validators.required],
    reportedOn: [null, Validators.required],
    latitude: [null, Validators.required],
    longitude: [null, Validators.required],
    participantsStatus: [null, Validators.required],
    participantsNominalCount: [null, Validators.required],
    accidentType: [null, Validators.required],
    description: [null],
  });

  constructor(private fb: FormBuilder,
    private service: DashboardService,
    private cd: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getAccidentTypes();
    this.getParticipantsCounts();
    this.getParticipantsStatuses();

    this.route.params.subscribe((params) => {
      this.trafficAccidentId = params['id'];
      this.isEdit = !!this.trafficAccidentId;

      if (this.isEdit) {
        this.service.getTrafficAccident(this.trafficAccidentId!).subscribe((response) => {
          this.form.patchValue(response)
          if (response.reportedOn) {
            const reportedOnDate = new Date(response.reportedOn);
            const formattedReportedOn = `${reportedOnDate.toISOString().slice(0, 16)}`;
            (this.form.get('reportedOn') as any).setValue(formattedReportedOn);
          }
          this.disableFormFields();
        });
      } else {
        this.form.patchValue({
          latitude: this.navigation?.extras?.state?.lat,
          longitude: this.navigation?.extras?.state?.lng
        });
      }
    });
  }

  private disableFormFields() {
    this.form.disable();
    this.form.get('description')?.enable();
  }

  formatEnumResponse(text: string) {
    const regex = /([A-Z])(?=[a-z])/g;
    return text.replace(regex, " $1")
  }

  getAccidentTypes() {
    this.service.getAccidentTypes().subscribe({
      next: response => {
        this.accidentTypes = response
        this.cd.detectChanges();
      },
      error: (error: any) => console.log(error)
    })
  }

  getParticipantsCounts() {
    this.service.getParticipantsCounts().subscribe({
      next: response => {
        this.participantsNominalCounts = response
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.message, "Ooops!")
    })
  }

  getParticipantsStatuses() {
    this.service.getParticipantsStatuses().subscribe({
      next: response => {
        this.participantsStatuses = response
        this.cd.detectChanges();
      },
      error: error => this.toastr.error(error.toString(), "Ooops!")
    })
  }

  saveTrafficAccident() {
    if (this.form.valid) {
      if (this.isEdit) {
        this.service.updateTrafficAccidentDescription(this.trafficAccidentId!, this.formControls.description.value).subscribe({
          next: _ => {
            this.toastr.success("Created a new traffic accident!");
            this.router.navigate(['home'])
          },
          error: error => this.toastr.error(error.message, "Ooops!")
        });
      } else {
        this.service.createTrafficAccident(this.form.value).subscribe({
          next: _ => {
            this.toastr.success("Created a new traffic accident!");
            this.router.navigate(['home'])
          },
          error: error => this.toastr.error(error.message, "Ooops!")
        })
      }
    }
  }

  get formControls() {
    return this.form.controls;
  }

  handleCancelClicked() {
    this.router.navigateByUrl('/home');
  }
}
