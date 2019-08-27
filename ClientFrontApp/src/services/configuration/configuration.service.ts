import { Injectable } from '@angular/core';

import { IServerConfiguration } from '../../models/IConfiguration';
import { EnvService } from '../environment/env.service';

@Injectable()
export class ConfigurationService {

  private configuration: IServerConfiguration;

  constructor(private envService: EnvService) {
  }

  loadConfig(): Promise<boolean> {
    this.configuration = {
      AppAdress : this.envService.appAddress,
      ApiAddress: this.envService.apiAddress,
      IdentityServerAddress: this.envService.identityServerAddress,
      ClientId: this.envService.clientId
    } as IServerConfiguration;

    console.log(`Configuration loaded for '${this.envService.envName}' environment.`);

    return new Promise<boolean>((resolve) => resolve());
  }

  get AppAddress() {
    return this.configuration.AppAdress;
  }

  get ApiAddress() {
    return this.configuration.ApiAddress;
  }

  get IdentityServerAddress() {
    return this.configuration.IdentityServerAddress;
  }

  get ClientId() {
    return this.configuration.ClientId;
  }
}
