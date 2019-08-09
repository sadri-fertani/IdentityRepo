import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthModule } from '../lib/auth/modules/auth.module';
import { OidcSecurityService } from '../lib/auth/services/oidc.security.service';
import { OidcConfigService, ConfigResult } from '../lib/auth/services/oidc.security.config.service';
import { OpenIdConfiguration } from '../lib/auth/models/auth.configuration';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { PageNotFoundComponent } from './page-not-found.component';

import { ConfigurationService } from "../services/configuration/configuration.service";

import { AuthService } from '../services/auth.service';
import { RepositoryCamp } from '../repositories/RepositoryCamp';

import { TokenInterceptor } from '../services/token.interceptor';
import { EnvServiceProvider } from 'src/services/environment/env.service.provider';

export function loadConfig(oidcConfigService: OidcConfigService) {
  console.log('APP_INITIALIZER STARTING');
  return () => oidcConfigService.load_using_stsServer('http://localhost/IdentityServer');
}

const appInitializerFn = (appConfig: ConfigurationService) => {
  return () => {
    return appConfig.loadConfig();
  };
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AuthModule.forRoot(),
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    RepositoryCamp,
    AuthService,
    OidcSecurityService,
    ConfigurationService,
    OidcConfigService,
    EnvServiceProvider,
    {
      provide: 'ORIGIN_URL',
      useFactory: getBaseUrl
    },
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [OidcConfigService],
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializerFn,
      multi: true,
      deps: [ConfigurationService]
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
  constructor(
    private oidcSecurityService: OidcSecurityService,
    private oidcConfigService: OidcConfigService
  ) {
    this.oidcConfigService.onConfigurationLoaded.subscribe((configResult: ConfigResult) => {

      const openIdConfiguration = {
        stsServer: 'http://localhost/IdentityServer',
        redirect_url: getBaseUrl(),
        client_id: 'ng',
        response_type: 'id_token token',
        scope: 'openid profile apiApp',
        post_login_route: '/',
        post_logout_redirect_uri: getBaseUrl() + 'home',
        forbidden_route: '/forbidden',
        unauthorized_route: '/unauthorized',
        silent_renew: true,
        silent_renew_url: getBaseUrl() + '/silent-renew.html',
        //auto_userinfo: true,
        start_checksession: true,
        log_console_warning_active: true,
        log_console_debug_active: false,
        max_id_token_iat_offset_allowed_in_seconds: 10 * 60 * 60,
        history_cleanup_off: true,
        iss_validation_off: true
      } as OpenIdConfiguration;

      this.oidcSecurityService.setupModule(
        openIdConfiguration,
        configResult.authWellknownEndpoints
      );

      this.oidcSecurityService.authorize(url => {
        console.log('URL', url)
      })

    });

    console.log('APP STARTING');
  }
}

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}