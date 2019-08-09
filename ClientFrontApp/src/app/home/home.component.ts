import { Component } from '@angular/core';
import { EnvService } from 'src/services/environment/env.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private envService: EnvService) {
    console.log('**************');
    console.log(envService.apiUrl);
    console.log(envService.enableDebug);
    console.log(envService.envName);
    console.log('**************');
  }
}
