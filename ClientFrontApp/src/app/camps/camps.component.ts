import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { RepositoryCamp } from '../../repositories/RepositoryCamp';

import { ICamp } from '../../models/ICamp';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'camps',
  templateUrl: './camps.component.html'
})
export class CampsComponent {
  private camps: Array<ICamp>;
  private busy: boolean;

  public get Busy(): boolean {
    return this.busy;
  }

  public set Busy(value) {
    this.busy = value;
  }

  public get Camps(): Array<ICamp> {
    return this.camps;
  }

  public set Camps(value) {
    this.camps = value;
  }

  constructor(private repository: RepositoryCamp, private spinner: NgxSpinnerService) {
    this.Busy = true;
  }

  ngOnInit() {
    this.spinner.show();

    this.repository
      .findAll()
      .pipe(
        finalize(()=>{
          this.spinner.hide();
          this.Busy = false;
        })).subscribe(result => {
      this.Camps = result;
    }, error => console.error(error));
  }
}
