import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(public oidcSecurityService: OidcSecurityService) {
  }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // Content-type : Spécialement pour le bon fonctionnement des requête http POST
    request = request.clone({
      setHeaders:
      {
        'Content-Type': 'application/json',
        'X-Version': '1.1'
      }
    });

    // Injection du token (authentification avec l'API, si nécessaire)
    const token = this.oidcSecurityService.getToken();
    
    if (token !== '') {
      request = request.clone({
        setHeaders:
        {
          'Authorization': 'Bearer ' + token
        }
      });
    }

    return next.handle(request);
  }
}