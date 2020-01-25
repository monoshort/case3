import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BestellingOverzichtComponent } from './bestelling-overzicht.component';
import { WinkelwagenOverzichtComponent } from '../../components/winkelwagen-overzicht/winkelwagen-overzicht.component';
import { AdresComponent } from '../../components/adres/adres.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';
import { Bestelling } from '../../models/bestelling';
import { BestelService } from '../../services/bestel.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BestellingResult } from '../../models/bestellingresult';
import { AngularFontAwesomeModule } from 'angular-font-awesome';

describe('BestellingOverzichtComponent', () => {
  let component: BestellingOverzichtComponent;
  let fixture: ComponentFixture<BestellingOverzichtComponent>;
  let router: Router;
  let bestelService: BestelService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        ToastrModule.forRoot(),
        AngularFontAwesomeModule
      ],
      declarations: [
        BestellingOverzichtComponent,
        WinkelwagenOverzichtComponent,
        AdresComponent
      ],
      providers: [
        BestelService,
        {
          provide: 'BASE_URL',
          useValue: 'http://example.com'
        }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BestellingOverzichtComponent);
    component = fixture.componentInstance;
    router = TestBed.get(Router);
    bestelService = TestBed.get(BestelService);
    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should call the bestelService to bestel a bestelling', () => {
    // Arrange
    component.bestelling = { klant: { naam: 'klantenNaam' } } as Bestelling;
    const spyObj = spyOn(bestelService, 'bestel').and.returnValue(new Observable<BestellingResult>());
    fixture.detectChanges();

    // Act
    component.bestellen();

    // Assert
    expect(spyObj).toHaveBeenCalledWith(component.bestelling);
  });

  it('should navigate to the homepage if there isn\'t anything to display', () => {
    // Arrange
    component.bestelling = undefined;
    const navigateSpy = spyOn(router, 'navigate');

    // Act
    component.ngOnInit();

    // Assert
    expect(navigateSpy).toHaveBeenCalledWith(['']);
  });
});
