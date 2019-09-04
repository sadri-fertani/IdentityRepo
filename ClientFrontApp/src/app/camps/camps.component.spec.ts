import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CampsComponent } from './camps.component';
import { RepositoryCamp } from 'src/repositories/RepositoryCamp';
import { ConfigurationService } from 'src/services/configuration/configuration.service';
import { APP_INITIALIZER } from '@angular/core';
import { loadAuthenticationConfig } from '../app.module';
import { EnvServiceProvider } from 'src/services/environment/env.service.provider';
import { AuthService } from '../core/authentication/auth.service';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('CampsComponent', () => {
    let component: CampsComponent;
    let fixture: ComponentFixture<CampsComponent>;

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
                RepositoryCamp,
                ConfigurationService],
            declarations: [CampsComponent],
            imports: [FormsModule, HttpClientTestingModule, NgxSpinnerModule]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(CampsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
