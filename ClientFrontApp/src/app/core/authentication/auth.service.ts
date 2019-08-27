import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';

import { BaseService } from "../../shared/base.service";
import { ConfigurationService } from 'src/services/configuration/configuration.service';
import { Roles } from 'src/app/shared/roles.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private manager = null;
  public user: User | null;

  constructor(private http: HttpClient, private configService: ConfigurationService) {
    super();

    this.manager = new UserManager(this.getClientSettings());

    this.manager.getUser().then(user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  login() {
    return this.manager.signinRedirect();
  }

  async completeAuthentication() {
    this.user = await this.manager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());
  }

  register(userRegistration: any) {
    return this.http.post(this.configService.IdentityServerAddress + '/api/account', userRegistration).pipe(catchError(this.handleError));
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  isInRole(roleName: Roles): boolean {
    if (this.user == null || this.user.profile == null || this.user.profile.role == undefined || this.user.profile.role == null)
      return false;

    if (this.user.profile.role.constructor === Array) {
      return (this.user.profile.role as Array<string>).includes(roleName);
    } else {
      return (this.user.profile.role as string) === roleName;
    }
  }

  get authorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  get name(): string {
    return this.user != null ? this.user.profile.name : '';
  }

  signout() {
    this.manager.signoutRedirect();
  }

  private getClientSettings(): UserManagerSettings {

    return {
      authority: this.configService.IdentityServerAddress,
      client_id: this.configService.ClientId,
      redirect_uri: this.configService.AppAddress + 'auth-callback',
      post_logout_redirect_uri: this.configService.AppAddress,
      response_type: "id_token token",
      scope: "openid profile email api.scope",
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      silent_redirect_uri: this.configService.AppAddress + 'silent-refresh.html'
    };
  }
}