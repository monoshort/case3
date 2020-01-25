import { TestBed, getTestBed } from '@angular/core/testing';

import { OidcAuthGuard } from './oidc-auth-guard.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from '../services/auth.service';
import { ToastrModule } from 'ngx-toastr';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AppConfigProvider } from '../../app/providers/app-config-provider';

describe('OidcAuthGuard', () => {
  let injector: TestBed;
  let authService: AuthService;
  let guard: OidcAuthGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        OidcAuthGuard,
        { provide: 'BASE_URL', useValue: 'http://example.com' },
        AppConfigProvider
      ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        ToastrModule.forRoot()
      ]
    });
    injector = getTestBed();
    authService = injector.get(AuthService);
    guard = injector.get(OidcAuthGuard);
  });

  it('should be created', () => {
    // Assert
    expect(guard).toBeTruthy();
  });

  it('should allow the authenticated user to access app', (done) => {
    // Arrange
    spyOnProperty(authService, 'isLoggedIn', 'get').and.returnValue(true);
    spyOn(authService, 'login');

    // Act
    guard.canActivate({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot).then(data => {

      // Assert
      expect(data).toEqual(true);
      done();
    });

  });

  it('should redirect an unauthenticated user to the login route', (done) => {
    // Arrange
    spyOnProperty(authService, 'isLoggedIn', 'get').and.returnValue(false);
    spyOn(authService, 'login');

    // Act
    guard.canActivate({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot).then(data => {

      // Assert
      expect(data).toEqual(false);
      done();
    });
  });

});
