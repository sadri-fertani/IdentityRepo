import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { LoginComponent } from './login.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { ConfigurationService } from 'src/services/configuration/configuration.service';
import { EnvServiceProvider } from 'src/services/environment/env.service.provider';
import { APP_INITIALIZER } from '@angular/core';
import { loadAuthenticationConfig } from 'src/app/app.module';

describe('LoginComponent', () => {
  let el: HTMLElement;
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

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
        ConfigurationService],
      imports: [FormsModule, HttpClientTestingModule, NgxSpinnerModule],
      declarations: [LoginComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should have title "Login"', () => {
    expect(component.title).toEqual('Login');
  });

  it('should call the login method', () => {
    spyOn(component, 'login');
    el = fixture.debugElement.query(By.css('button')).nativeElement;
    el.click();
    expect(component.login).toHaveBeenCalled();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
