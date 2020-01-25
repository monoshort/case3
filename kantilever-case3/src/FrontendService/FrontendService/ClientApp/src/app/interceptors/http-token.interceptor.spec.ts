import { TestBed, getTestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthHttpInterceptor } from './http-token.interceptor';
import { AuthService } from '../../app/services/auth.service';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { ArtikelenDataService } from '../services/artikelen-data.service';
import { AppConfigProvider } from '../../app/providers/app-config-provider';
import { endpoints } from '../../constants/endpoints';

describe(`AuthHttpInterceptor`, () => {
  let service: ArtikelenDataService;
  let httpMock: HttpTestingController;
  let injector: TestBed;
  let authService: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        ToastrModule.forRoot()
      ],
      providers: [
        { provide: 'BASE_URL', useValue: 'http://example.com/' },
        ArtikelenDataService,
        AuthService,
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthHttpInterceptor,
          multi: true,
        },
        AppConfigProvider
      ],
    });

    injector = getTestBed();
    authService = injector.get(AuthService);
    service = injector.get(ArtikelenDataService);
    httpMock = injector.get(HttpTestingController);
  });

  it('should add an Authorization header to each request', () => {
    spyOnProperty(authService, 'tokenType', 'get').and.returnValues('bearer');
    spyOnProperty(authService, 'token', 'get').and.returnValues('12345678q2wertyui');

    service.getData().subscribe(response => {
      expect(response).toBeTruthy();
    });

    const httpRequest = httpMock.expectOne(`http://example.com/${endpoints.artikelen}`);

    expect(httpRequest.request.headers.has('Authorization')).toEqual(true);
  });

});
