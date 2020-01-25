import { async, getTestBed, TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';
import { AppConfigProvider } from '../../app/providers/app-config-provider';
import { Profile, User } from 'oidc-client';

describe('AuthService', () => {
  let service: AuthService;
  let injector: TestBed;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        ToastrModule.forRoot()
      ],
      providers: [
        AuthService,
        { provide: 'BASE_URL', useValue: 'http://dummy:1234' },
        AppConfigProvider
      ]
    });

    service = TestBed.get(AuthService);
  }));

  beforeEach(() => {
    injector = getTestBed();
    service = injector.get(AuthService);
    // Arrange
    service.user = {
      access_token: 'access',
      expired: true,
      expires_at: 9999999999999,
      expires_in: 9999999999999,
      id_token: 'unique',
      profile: {
        preferred_username: 'username'
      } as Profile,
      token_type: 'Bearer'
    } as User;
  });

  it('should be created', () => {
    // Assert
    expect(service).toBeTruthy();
  });

  it('should be able to get the username', () => {
    // Assert
    expect(service.username).toBe('username');
  });

  it('should return null if the username isnt avaliable', () => {
    // Assert
    service.user = null;
    expect(service.username).toBe(null);
  });

  it('should be able to get the token', () => {
    // Assert
    expect(service.token).toBe('access');
  });

  it('should return null if the token isnt avaliable', () => {
    // Assert
    service.user = null;
    expect(service.token).toBe(null);
  });

  it('should be able to get the token type', () => {
    // Assert
    expect(service.tokenType).toBe('Bearer');
  });

  it('should return null if the token type isnt avaliable', () => {
    // Assert
    service.user = null;
    expect(service.tokenType).toBe(null);
  });

  it('should not see an exipred login as a logged in user', () => {
    // Act
    service.user = {
      expired: true
    } as User;

    // Assert
    expect(service.isLoggedIn).toBe(false);
  });

  it('should not see an exipred login as a logged in user', () => {
    // Act
    service.user = {
      expired: false
    } as User;

    // Assert
    expect(service.isLoggedIn).toBe(true);
  });

});
