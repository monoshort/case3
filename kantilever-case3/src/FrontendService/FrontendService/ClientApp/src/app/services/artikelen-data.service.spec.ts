import { ArtikelenDataService } from './artikelen-data.service';
import { async, TestBed, getTestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AppConfigProvider } from '../../app/providers/app-config-provider';
import { endpoints } from '../../constants/endpoints';

describe('ArtikelenDataService', () => {
  let httpTestingController: HttpTestingController;
  let injector: TestBed;
  let service: ArtikelenDataService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        ReactiveFormsModule,
        HttpClientTestingModule,

      ],
      declarations: [],
      providers: [
        ArtikelenDataService,
        {
          provide: 'BASE_URL',
          useValue: 'http://localhost:5000/'
        },
        AppConfigProvider
      ]
    });
  }));

  beforeEach(() => {
    injector = getTestBed();
    httpTestingController = injector.get(HttpTestingController);
    service = injector.get(ArtikelenDataService);
  });

  it(`should be able to make a call to http://localhost:5000/${endpoints.artikelen}`, () => {
    // Act
    service.getData().subscribe();

    // Assert
    const req = httpTestingController.expectOne(`http://localhost:5000/${endpoints.artikelen}`);

    req.flush([]);
  });

  it(`should be able to make a call to http://localhost:5000/${endpoints.artikelen}{id}`, () => {
    // Act
    service.getArtikelDetails(3).subscribe();

    // Assert
    const req = httpTestingController.expectOne(`http://localhost:5000/${endpoints.artikelen}/3`);

    req.flush([]);
  });
});
