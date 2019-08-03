import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

//import { AuthService } from '../../services/auth.service';
//import { OidcSecurityService, AuthorizationResult, AuthorizationState } from 'angular-auth-oidc-client';
import { OidcSecurityService } from '../../lib/auth/services/oidc.security.service';
import { Router } from '@angular/router';
import { AuthorizationResult } from 'src/lib/auth/models/authorization-result';
import { AuthorizationState } from 'src/lib/auth/models/authorization-state.enum';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.less']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  isAuthorizedSubscription: Subscription;
  isAuthorized: boolean;
  isExpanded = false;
  data:any = null;

  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {

    if (this.oidcSecurityService.moduleSetup) {
      this.doCallbackLogicIfRequired();
    } else {
      this.oidcSecurityService.onModuleSetup.subscribe(() => {
        this.doCallbackLogicIfRequired();
      });
    }

    this.oidcSecurityService.onAuthorizationResult.subscribe(
      (authorizationResult: AuthorizationResult) => {
        this.onAuthorizationResultComplete(authorizationResult);
      });
  }
  private onAuthorizationResultComplete(authorizationResult: AuthorizationResult) {

    console.log('Auth result received AuthorizationState:'
      + authorizationResult.authorizationState
      + ' validationResult:' + authorizationResult.validationResult);

    this.oidcSecurityService.getUserData().subscribe(
      (data: any) => {
        this.data = data;
        console.log('User info', this.data);
      });

    if (authorizationResult.authorizationState === AuthorizationState.unauthorized) {
      if (window.parent) {
        // sent from the child iframe, for example the silent renew
        this.router.navigate(['/unauthorized']);
      } else {
        window.location.href = '/unauthorized';
      }
    }
  }
  private doCallbackLogicIfRequired() {
    if (window.location.hash) {
      this.oidcSecurityService.authorizedImplicitFlowCallback();
    }
  }
  ngOnInit() {
    this.isAuthorizedSubscription = this.oidcSecurityService.getIsAuthorized().subscribe(
      (isAuthorized: boolean) => {

        console.log('isAuthorized', isAuthorized);

        this.isAuthorized = isAuthorized;

        if (this.isAuthorized) {
          console.log('isAuthorized getting data');
          //this.getData();
        }
      });
  }

  ngOnDestroy(): void {
    if (this.isAuthorizedSubscription) {
      this.isAuthorizedSubscription.unsubscribe();
    }
  }

  public login() {
    this.oidcSecurityService.authorize();//.login();
  }

  public refreshSession() {
    this.oidcSecurityService.authorize();
  }

  public logout() {
    this.oidcSecurityService.logoff();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
