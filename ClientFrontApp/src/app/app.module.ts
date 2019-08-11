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

export function loadAuthenticationConfig(oidcConfigService: OidcConfigService, appConfig: ConfigurationService) {
  console.log('APP_INITIALIZER STARTING');
  appConfig.loadConfig();
  return () => oidcConfigService.load_using_stsServer(appConfig.identityServerAddress);
}

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
      provide: APP_INITIALIZER,
      useFactory: loadAuthenticationConfig,
      deps: [OidcConfigService, ConfigurationService],
      multi: true,
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
    private oidcConfigService: OidcConfigService,
    private appConfig: ConfigurationService) {
    this.oidcConfigService.onConfigurationLoaded.subscribe((configResult: ConfigResult) => {

      const baseUrl = document.getElementsByTagName('base')[0].href;

      const openIdConfiguration = {
        stsServer: configResult.customConfig.stsServer,
        redirect_url: baseUrl,
        client_id: this.appConfig.clientId,
        response_type: 'id_token token',
        scope: 'openid profile apiApp',
        post_login_route: '/',
        post_logout_redirect_uri: `${baseUrl}home`,
        forbidden_route: '/forbidden',
        unauthorized_route: '/unauthorized',
        silent_renew: true,
        silent_renew_url: `${baseUrl}silent-renew.html`,
        auto_userinfo: true,
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

      this.oidcSecurityService.authorize(() => {
        console.log('OIDC CONFIGURATION LOADED')
      })

    });
  }
}
