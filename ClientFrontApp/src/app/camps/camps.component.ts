import { Component } from '@angular/core';

import { RepositoryCamp } from '../../repositories/RepositoryCamp';

import { ICamp } from '../../models/ICamp';

@Component({
  selector: 'camps',
  templateUrl: './camps.component.html'
})
export class CampsComponent {
  private camps: Array<ICamp>;

  public get Camps(): Array<ICamp> {
    return this.camps;
  }

  public set Camps(value) {
    this.camps = value;
  }

  constructor(private repository: RepositoryCamp) {
    this.repository.findAll().subscribe(result => {
      this.Camps = result;
    }, error => console.error(error));
  }
}
