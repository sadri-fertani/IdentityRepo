import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CountriesComponent } from './countries.component';

describe('IndexComponent', () => {
    let component: CountriesComponent;
    let fixture: ComponentFixture<CountriesComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [CountriesComponent],
            imports: [NgxSpinnerModule]
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
