import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { CampsRoutingModule } from './camps-routing.module';

import { CampsComponent } from './camps.component';


@NgModule({
  declarations: [CampsComponent],
  imports: [
    CommonModule,
    CampsRoutingModule,
    SharedModule
  ]
})
export class CampsModule { }