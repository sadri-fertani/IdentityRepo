import { TestBed } from '@angular/core/testing';
import { EnvService } from './env.service';

describe('EnvService', () => {
    beforeEach(() => TestBed.configureTestingModule({}));

    it('Should be created', () => {
        const service: EnvService = TestBed.get(EnvService);
        expect(service).toBeTruthy();
    });
});