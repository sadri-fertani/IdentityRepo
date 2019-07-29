import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';

import { RoutingModule } from './app.routing.module';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CampsComponent } from './camps/camps.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { PageNotFoundComponent } from './page.not.found.component';

import { ConfigurationService } from "../services/configuration/configuration.service";

import { AuthService } from '../services/auth.service';
import { RepositoryCamp } from '../repositories/RepositoryCamp';

import { TokenInterceptor } from '../services/token.interceptor'

const appInitializerFn = (appConfig: ConfigurationService) => {
  return () => {
    return appConfig.loadConfig();
  };
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CampsComponent,
    UnauthorizedComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AuthModule.forRoot(),
    HttpClientModule,
    FormsModule,
    RoutingModule
  ],
  providers: [
    {
      provide: 'ORIGIN_URL',
      useFactory: getBaseUrl
    },
    RepositoryCamp,
    AuthService,
    OidcSecurityService,
    ConfigurationService,
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
export class AppModule { }

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}
