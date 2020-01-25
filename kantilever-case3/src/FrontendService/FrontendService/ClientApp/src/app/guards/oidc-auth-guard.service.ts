import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class OidcAuthGuard implements CanActivate {

  constructor(private readonly authService: AuthService) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
      if (this.authService.isLoggedIn) {
        resolve(true);
      } else {
        this.authService.login();
        resolve(false);
      }
    });
  }
}
