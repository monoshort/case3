import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private readonly _authService: AuthService) {
  }

  public toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  public login(): void {
    this._authService.login();
  }

  public logout(): void {
    this._authService.logout();
  }

  public isLoggedIn(): boolean {
    return this._authService.isLoggedIn;
  }
}
