import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CountriesComponent } from './countries.component';
import { RepositoryCountries } from 'src/repositories/RepositoryCountries';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { APP_INITIALIZER } from '@angular/core';
import { loadAuthenticationConfig } from '../app.module';
import { ConfigurationService } from 'src/services/configuration/configuration.service';
import { EnvServiceProvider } from 'src/services/environment/env.service.provider';
import { AuthService } from '../core/authentication/auth.service';
import { ActivatedRoute } from '@angular/router';

describe('CountriesComponent', () => {
    let component: CountriesComponent;
    let fixture: ComponentFixture<CountriesComponent>;

    const fakeActivatedRoute = {
        snapshot: {
            data: {}
        }
    } as ActivatedRoute;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            providers: [
                {
                    provide: APP_INITIALIZER,
                    useFactory: loadAuthenticationConfig,
                    deps: [ConfigurationService],
                    multi: true,
                },
                EnvServiceProvider,
                AuthService,
                ConfigurationService,
                {
                    provide: ActivatedRoute,
                    useValue: fakeActivatedRoute
                },
                RepositoryCountries],
            declarations: [CountriesComponent],
            imports: [FormsModule, HttpClientTestingModule, NgxSpinnerModule]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CountriesComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
