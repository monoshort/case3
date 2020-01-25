import { TestBed, async, getTestBed } from '@angular/core/testing';

import { KlantService } from './klant.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import {endpoints} from '../../constants/endpoints';

describe('KlantService', () => {
  let httpTestingController: HttpTestingController;
  let injector: TestBed;
  let service: KlantService;

  beforeEach(async(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule,
      RouterTestingModule,
      ToastrModule.forRoot()
    ],
    declarations: [],
    providers: [
      {
        provide: 'BASE_URL',
        useValue: 'http://localhost:5000/'
      }
    ],
  })));

  beforeEach(() => {
    injector = getTestBed();
    service = injector.get(KlantService);
    httpTestingController = injector.get(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it(`should be able to make a call to http://localhost:5000/${endpoints.klanten}`, () => {
    // Act
    service.getKlant('username').subscribe();

    // Assert
    const req = httpTestingController.expectOne(`http://localhost:5000/${endpoints.klanten}/username`);

    req.flush([]);
  });
});
