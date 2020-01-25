import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../app/services/auth.service';

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {

    constructor(
        private readonly _authservice: AuthService,
    ) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const newRequest = req.clone({
            headers: req.headers.set('Authorization', `${this._authservice.tokenType} ${this._authservice.token}`),
        });
        return next.handle(newRequest);
    }
}
