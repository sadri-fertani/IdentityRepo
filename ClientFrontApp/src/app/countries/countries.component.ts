import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { RepositoryCountries } from '../../repositories/RepositoryCountries';

import { finalize } from 'rxjs/operators';
import { ICountry } from 'src/models/ICountry';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'countries',
  templateUrl: './countries.component.html'
})

export class CountriesComponent {
  private countries: Array<ICountry>;
  private busy: boolean;
  private title: string;

  public get Busy(): boolean {
    return this.busy;
  }

  public set Busy(value) {
    this.busy = value;
  }

  public get Countries(): Array<ICountry> {
    return this.countries;
  }

  public set Countries(value) {
    this.countries = value;
  }

  public get Title(): string {
    return this.title;
  }

  public set Title(value) {
    this.title = value;
  }

  constructor(
    private repository: RepositoryCountries,
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute) {

    this.Title = this.route.snapshot.data.title as string;
    this.Busy = true;
  }

  ngOnInit() {
    this.spinner.show();

    this.repository
      .findAll()
      .pipe(
        finalize(() => {
          this.spinner.hide();
          this.Busy = false;
        })).subscribe(result => {
          this.Countries = result;
        }, error => console.error(error));
  }
}
