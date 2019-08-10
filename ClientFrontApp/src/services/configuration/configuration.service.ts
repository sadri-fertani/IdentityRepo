import { Injectable } from '@angular/core';

import { IServerConfiguration } from '../../models/IConfiguration';
import { EnvService } from '../environment/env.service';

@Injectable()
export class ConfigurationService {

  private configuration: IServerConfiguration;

  constructor(private envService: EnvService) {
  }

  loadConfig() {
    console.log(`Load configuration for '${this.envService.envName}' environment.`);

    this.configuration = {
      ApiAddress: this.envService.apiAddress,
      IdentityServerAddress: this.envService.identityServerAddress
    } as IServerConfiguration;
  }

  get apiAddress() {
    return this.configuration.ApiAddress;
  }

  get identityServerAddress() {
    return this.configuration.IdentityServerAddress;
  }
}
