import { Injectable, OnInit, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription';
import { ConfigurationService } from "./configuration/configuration.service";

import { OidcSecurityService, OpenIDImplicitFlowConfiguration } from 'angular-auth-oidc-client';

@Injectable()
export class AuthService implements OnInit, OnDestroy {
  isAuthorizedSubscription: Subscription;
  isAuthorized: boolean;

  constructor(public oidcSecurityService: OidcSecurityService, private http: HttpClient, @Inject('ORIGIN_URL') originUrl: string, configuration: ConfigurationService) {
    
    const openIdImplicitFlowConfiguration = {
      stsServer: configuration.identityServerAddress,
      redirect_url: originUrl,
      client_id: 'ng',
      response_type: 'id_token token',
      scope: 'openid profile apiApp',
      post_logout_redirect_uri: originUrl + 'home',
      forbidden_route: '/forbidden',
      unauthorized_route: '/unauthorized',
      auto_userinfo: true,
      log_console_warning_active: true,
      log_console_debug_active: false,
      max_id_token_iat_offset_allowed_in_seconds: 10
    } as OpenIDImplicitFlowConfiguration;

    this.oidcSecurityService.setupModule(openIdImplicitFlowConfiguration);

    if (this.oidcSecurityService.moduleSetup) {
      this.doCallbackLogicIfRequired();
    } else {
      this.oidcSecurityService.onModuleSetup.subscribe(() => {
        this.doCallbackLogicIfRequired();
      });
    }
  }

  ngOnInit() {
    this.isAuthorizedSubscription = this.oidcSecurityService.getIsAuthorized().subscribe(
      (isAuthorized: boolean) => {
        this.isAuthorized = isAuthorized;
      });
  }

  ngOnDestroy(): void {
    this.isAuthorizedSubscription.unsubscribe();
    this.oidcSecurityService.onModuleSetup.unsubscribe();
  }

  getIsAuthorized(): Observable<boolean> {
    return this.oidcSecurityService.getIsAuthorized();
  }

  login() {
    console.log('start login');
    this.oidcSecurityService.authorize();
  }

  refreshSession() {
    console.log('start refreshSession');
    this.oidcSecurityService.authorize();
  }

  logout() {
    console.log('start logoff');
    this.oidcSecurityService.logoff();
  }

  private doCallbackLogicIfRequired() {
    if (typeof location !== "undefined" && window.location.hash) {
      this.oidcSecurityService.authorizedCallback();
    }
  }
}
