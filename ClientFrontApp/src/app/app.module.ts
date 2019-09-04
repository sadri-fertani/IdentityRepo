import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';

import { AuthCallbackComponent } from './auth-callback/auth-callback.component';

/* Module Imports */
import { CoreModule } from './core/core.module';
import { HomeModule } from './home/home.module';
import { AccountModule } from './account/account.module';
import { ShellModule } from './shell/shell.module';
import { CampsModule } from './camps/camps.module';
import { CountriesModule } from './countries/countries.module';
import { SharedModule } from './shared/shared.module';
import { TokenInterceptor } from 'src/services/token.interceptor';
import { RepositoryCamp } from 'src/repositories/RepositoryCamp';
import { RepositoryCountries } from 'src/repositories/RepositoryCountries';
import { ConfigurationService } from 'src/services/configuration/configuration.service';
import { EnvServiceProvider } from 'src/services/environment/env.service.provider';

export function loadAuthenticationConfig(appConfig: ConfigurationService) {
  console.log('Application initializer starting');
  return () => appConfig.loadConfig();
}

@NgModule({
  declarations: [
    AppComponent,
    AuthCallbackComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CoreModule,
    HomeModule,
    AccountModule,
    CampsModule,
    CountriesModule,
    AppRoutingModule,
    ShellModule,
    SharedModule
  ],
  providers: [
    RepositoryCamp,
    RepositoryCountries,
    ConfigurationService,
    EnvServiceProvider,
    {
      provide: APP_INITIALIZER,
      useFactory: loadAuthenticationConfig,
      deps: [ConfigurationService],
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
export class AppModule { }
