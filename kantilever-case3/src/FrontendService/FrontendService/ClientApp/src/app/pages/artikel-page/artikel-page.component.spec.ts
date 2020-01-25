import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArtikelPageComponent } from './artikel-page.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BestelService } from '../../services/bestel.service';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ArtikelenDataService } from '../../services/artikelen-data.service';
import { ArtikelDetails } from '../../models/artikel';

describe('ArtikelPageComponent', () => {
  let component: ArtikelPageComponent;
  let fixture: ComponentFixture<ArtikelPageComponent>;

  let winkelwagenService: WinkelwagenService;
  let artikelenDataService: ArtikelenDataService;
  let toastrService: ToastrService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ArtikelPageComponent
      ], imports: [
        RouterTestingModule,
        ReactiveFormsModule,
        ToastrModule.forRoot(),
        HttpClientTestingModule,
        AngularFontAwesomeModule,
        FormsModule,
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                artikelId: 123
              }
            }
          }
        },
        BestelService,
        WinkelwagenService,
        ArtikelenDataService,
        ToastrService,
        { provide: 'BASE_URL', useValue: 'http://example.com' }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArtikelPageComponent);
    component = fixture.componentInstance;

    winkelwagenService = TestBed.get(WinkelwagenService);
    artikelenDataService = TestBed.get(ArtikelenDataService);
    toastrService = TestBed.get(ToastrService);

    fixture.detectChanges();
  });

  it('should create', () => {
    // Act
    expect(component).toBeTruthy();
  });

  it('should get artikelId from the parameters and call the artikelservice with this', () => {
    // Arrange
    spyOn(artikelenDataService, 'getArtikelDetails').and.returnValue(new Observable<ArtikelDetails>());

    // Act
    component.ngOnInit();

    // Assert
    expect(artikelenDataService.getArtikelDetails).toHaveBeenCalledWith(123);
  });

  it('should call the toastrservice with the article and amount that was added to basket', () => {
    // Arrange
    spyOn(toastrService, 'info');
    component.aanWinkelwagenToevoegForm.get('hoeveelheid').setValue('18');
    component.artikel = { naam: 'fiets' } as ArtikelDetails;

    // Act
    component.addToWinkelwagen();

    // Assert
    expect(toastrService.info).toHaveBeenCalledWith('18 x fiets', 'Toegevoegd aan winkelwagen');
  });

  it('should call the WinkelwagenService.add with a "bike" and 12 as parameters.', () => {
    // Arrange
    spyOn(winkelwagenService, 'addArtikel');
    component.aanWinkelwagenToevoegForm.get('hoeveelheid').setValue('12');
    component.artikel = { naam: 'bike' } as ArtikelDetails;

    // Act
    component.addToWinkelwagen();

    // Assert
    expect(winkelwagenService.addArtikel).toHaveBeenCalledWith({ naam: 'bike' } as ArtikelDetails, 12);
  });
});
