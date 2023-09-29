import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { MapComponent } from './map/map.component';

@NgModule({
  declarations: [
    DashboardComponent,
    MapComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    DashboardComponent
  ]
})
export class DashboardModule { }
