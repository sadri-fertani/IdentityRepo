import { Component } from '@angular/core';

import { ICamp } from '../../models/ICamp';

import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'camps-free',
  templateUrl: './camps.component.html'
})
export class CampsFreeComponent {
  private camps: Array<ICamp>;

  private get Camps(): Array<ICamp> {
    return this.camps;
  }

  private set Camps(value) {
    this.camps = value;
  }

  constructor(private http: HttpClient) {
    this.http.get('http://localhost:5001/api/camps').subscribe(result => {
      this.Camps = result as Array<ICamp>
    });
  }
}
