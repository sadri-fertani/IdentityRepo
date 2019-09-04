import { of } from 'rxjs';
import { Roles } from '../roles.enum';

export class MockAuthService {

  authNavStatus$ = of(true);

  register(userRegistration: any) {
    return of('');
  }

  isAuthenticated(): boolean {
    return false;
  }

  get authorizationHeaderValue(): string {
    return '';
  }

  isInRole(roleName: Roles): boolean {
    return false;
  }
}