import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  constructor(
    private cookieService : CookieService,
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log(request);
    let modifiedRequest : HttpRequest<any>

    if (request.url.includes('/api/token')) {

      modifiedRequest = request.clone({
        setHeaders: {
          'Authorization': this.cookieService.get('bearer_token')
        }
      })
      return next.handle(modifiedRequest);
    } else {
      return next.handle(request);
    }
  }
}
