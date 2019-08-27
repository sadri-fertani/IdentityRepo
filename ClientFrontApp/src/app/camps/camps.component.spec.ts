import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CampsComponent } from './camps.component';

describe('IndexComponent', () => {
    let component: CampsComponent;
    let fixture: ComponentFixture<CampsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [CampsComponent],
            imports: [NgxSpinnerModule]
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
