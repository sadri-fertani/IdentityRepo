import { Injectable } from '@angular/core';

import { IServerConfiguration } from '../../models/IConfiguration';

@Injectable()
export class ConfigurationService {

  private configuration: IServerConfiguration;

  constructor() {
  }

  loadConfig() {
    return new Promise((resolve) => {
      resolve();
    }).then(() => {
      this.configuration = {
        ApiAddress: 'http://localhost:5001/api/',
        IdentityServerAddress: 'http://localhost/IdentityServer'
      } as IServerConfiguration;
    }).catch((err) => {
      console.log(err);
    });
  }

  get apiAddress() {
    return this.configuration.ApiAddress;
  }

  get identityServerAddress() {
    return this.configuration.IdentityServerAddress;
  }
}
