import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CampsRoutingModule } from './camps-routing.module';

import { CampsComponent } from './camps.component';


@NgModule({
  declarations: [CampsComponent],
  imports: [
    CommonModule,
    CampsRoutingModule
  ]
})
export class CampsModule { }
