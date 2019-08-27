import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CountriesComponent } from './countries.component';
import { Shell } from '../shell/shell.service';

import { AuthGuard } from '../core/authentication/auth.guard';

const routes: Routes = [
  Shell.childRoutes([
    {
      path: 'countries',
      component: CountriesComponent,
      canActivate: [AuthGuard],
      data: { 'title': 'List of countries' }
    }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CountriesRoutingModule { }
