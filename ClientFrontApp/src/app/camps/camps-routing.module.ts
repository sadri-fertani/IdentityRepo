import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CampsComponent } from './camps.component';
import { Shell } from './../shell/shell.service';

import { AuthGuard } from '../core/authentication/auth.guard';

const routes: Routes = [
  Shell.childRoutes([
    { path: 'camps', component: CampsComponent, canActivate: [AuthGuard] }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CampsRoutingModule { }
