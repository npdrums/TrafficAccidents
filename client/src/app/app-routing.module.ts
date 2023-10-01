import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TrafficAccidentFormComponent } from './dashboard/traffic-accident-form/traffic-accident-form.component';

const routes: Routes = [
  { path: "", redirectTo: "/home", pathMatch: "full" },
  {
    path: "home",
    component: DashboardComponent
  },
  { path: "traffic-accident", component: TrafficAccidentFormComponent },
  { path: "traffic-accident/:id", component: TrafficAccidentFormComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
