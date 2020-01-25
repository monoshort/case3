import { BestelService } from './bestel.service';
import { async, TestBed, getTestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { Bestelling } from '../models/bestelling';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { GemaakteBestelling } from '../models/gemaakte-bestelling';
import {endpoints} from '../../constants/endpoints';

describe('BestelService', () => {
  let httpTestingController: HttpTestingController;
  let service: BestelService;
  let injector: TestBed;
  let toastrService: ToastrService;
  let toastrSpy;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        ToastrModule.forRoot(),
      ],
      declarations: [],
      providers: [
        BestelService,
        {
          provide: 'BASE_URL',
          useValue: 'http://localhost:5000/'
        }
      ]
    });
  }));

  beforeEach(() => {
    injector = getTestBed();
    service = injector.get(BestelService);
    httpTestingController = injector.get(HttpTestingController);
    toastrService = injector.get(ToastrService);
    toastrSpy = spyOn(toastrService, 'warning');
  });

  it(`should be able to make a call to http://localhost:5000/${endpoints.bestellingen}`, () => {
    // Act
    service.bestel({} as Bestelling).subscribe();

    // Assert
    const req = httpTestingController.expectOne(`http://localhost:5000/${endpoints.bestellingen}`);

    req.flush([]);
  });

  it('should not call toastr when all bestellingen are goedgekeurd', () => {
    // Arrange
    const bestellingen = [
      {goedgekeurd : true},
      {goedgekeurd : true},
      {goedgekeurd : true},
      {goedgekeurd : true},
      {goedgekeurd : true},
    ] as GemaakteBestelling[];

    // Act
    service.checkBestellingenVoorNietGoedgekeurd(bestellingen);

    // Assert
    expect(toastrSpy).toHaveBeenCalledTimes(0);
  });

  it('should call toastr when not all bestellingen are goedgekeurd', () => {
    // Arrange
    const bestellingen = [
      {goedgekeurd : true},
      {goedgekeurd : true},
      {goedgekeurd : false},
      {goedgekeurd : true},
      {goedgekeurd : true},
    ] as GemaakteBestelling[];

    // Act
    service.checkBestellingenVoorNietGoedgekeurd(bestellingen);

    // Assert
    expect(toastrSpy).toHaveBeenCalledTimes(1);
  });
});
