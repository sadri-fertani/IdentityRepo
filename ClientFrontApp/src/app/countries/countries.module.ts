import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { CountriesRoutingModule } from './countries-routing.module';

import { CountriesComponent } from './countries.component';

@NgModule({
  declarations: [CountriesComponent],
  imports: [
    CommonModule,
    CountriesRoutingModule,
    SharedModule
  ]
})
export class CountriesModule { }