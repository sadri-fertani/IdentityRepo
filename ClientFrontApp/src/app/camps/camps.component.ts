import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ConfigurationService } from "../configuration/configuration.service";

@Component({
  selector: 'camps',
  templateUrl: './camps.component.html'
})
export class CampsComponent {
  public camps: Camp[];

  constructor(authService: AuthService, configuration: ConfigurationService) {
    authService.get(configuration.apiAddress + 'camps').subscribe(result => {
      this.camps = result as Camp[];
    }, error => console.error(error));
  }
}

interface Camp {
  name: string;
  moniker: number;
  eventDate: Date;
  length: number;
}
