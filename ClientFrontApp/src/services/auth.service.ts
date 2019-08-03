import { Injectable, OnInit, OnDestroy, Inject } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { take, filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { AuthorizationResult } from 'src/lib/auth/models/authorization-result';
import { OidcSecurityService } from 'src/lib/auth/services/oidc.security.service';

//import { OidcSecurityService, AuthWellKnownEndpoints, AuthorizationResult } from '../lib/auth/data-services/oidc-data.service';

@Injectable()
export class AuthService implements OnInit, OnDestroy {
  isAuthorizedSubscription: Subscription;
  isAuthenticated: boolean;
  userData: any;
  private _onModuleSetup = new Subject<boolean>();
  private _onCheckSessionChanged = new Subject<boolean>();
  private _onAuthorizationResult = new Subject<AuthorizationResult>();

  public get onModuleSetup(): Observable<boolean> {
    return this._onModuleSetup.asObservable();
  }

  constructor(public oidcSecurityService: OidcSecurityService) {

    this.oidcSecurityService.getIsModuleSetup().pipe(
      filter((isModuleSetup: boolean) => isModuleSetup),
      take(1)
    ).subscribe((isModuleSetup: boolean) => {
      console.log('isModuleSetup : ', isModuleSetup)
      this.doCallbackLogicIfRequired();
    });

    // if (this.oidcSecurityService.moduleSetup) {
    //   this.doCallbackLogicIfRequired();
    // } else {
    //   this.oidcSecurityService.onModuleSetup.subscribe(() => {
    //     this.doCallbackLogicIfRequired();
    //   });
    // }
  }

  ngOnInit() {
    this.oidcSecurityService.getIsAuthorized().subscribe(auth => {
      this.isAuthenticated = auth;
    });

    this.oidcSecurityService.getUserData().subscribe(userData => {
      this.userData = userData;
    });
  }

  ngOnDestroy(): void {
    this.isAuthorizedSubscription.unsubscribe();
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
    this.oidcSecurityService.refreshSession().subscribe(() => { });
  }

  logout() {
    console.log('start logoff');
    this.oidcSecurityService.logoff();
  }

  private doCallbackLogicIfRequired() {
    // Will do a callback, if the url has a code and state parameter.
    this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
  }
}
