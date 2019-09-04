import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ConfigurationService } from 'src/services/configuration/configuration.service';

import { AuthService } from './auth.service';
import { APP_INITIALIZER } from '@angular/core';
import { loadAuthenticationConfig } from 'src/app/app.module';
import { EnvServiceProvider } from 'src/services/environment/env.service.provider';

describe('AuthService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      {
        provide: APP_INITIALIZER,
        useFactory: loadAuthenticationConfig,
        deps: [ConfigurationService],
        multi: true,
      },
      EnvServiceProvider,
      AuthService,
      ConfigurationService
    ],
    imports: [
      HttpClientTestingModule
    ],
  }));

  it('should be created', () => {
    const service: AuthService = TestBed.get(AuthService);
    expect(service).toBeTruthy();
  });
});
