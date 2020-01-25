import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ArtikelOverzichtComponent } from './artikel-overzicht.component';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DebugElement } from '@angular/core';
import { ArtikelenDataService } from '../../services/artikelen-data.service';
import { Observable } from 'rxjs';
import { Artikel } from '../../models/artikel';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ArtikelComponent } from '../../components/artikel/artikel.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { ZoekfilterPipe } from '../../pipes/zoekfilter.pipe';

describe('ArtikelOverzichtComponent', () => {
  let component: ArtikelOverzichtComponent;
  let artikelenService: ArtikelenDataService;
  let winkelwagenService: WinkelwagenService;
  let fixture: ComponentFixture<ArtikelOverzichtComponent>;

  let debugElement: DebugElement;
  let navigateSpy;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        AngularFontAwesomeModule,
        FormsModule
      ],
      declarations: [
        ArtikelOverzichtComponent,
        ArtikelComponent,
        ZoekfilterPipe
      ],
      providers: [
        WinkelwagenService,
        ArtikelenDataService,
        { provide: 'BASE_URL', useValue: 'http://example.com' }
      ],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArtikelOverzichtComponent);
    debugElement = fixture.debugElement;
    navigateSpy = spyOn(TestBed.get(Router), 'navigate');
    component = fixture.componentInstance;

    winkelwagenService = TestBed.get(WinkelwagenService);
    artikelenService = TestBed.get(ArtikelenDataService);

    fixture.detectChanges();
  });

  it('should be created', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should call getData() on artikelen service on init', () => {
    // Arrange
    spyOn(artikelenService, 'getData').and.returnValue(new Observable<Artikel[]>());

    // Act
    component.ngOnInit();

    // Assert
    expect(artikelenService.getData).toHaveBeenCalled();
  });

  it('should populate the artikelen on init', () => {
    // Arrange
    const artikel1: Artikel = {
      naam: 'Fiets',
      id: 1,
      afbeeldingUrl: 'leuke_fiets.jpg',
      beschikbaarAantal: 6,
      prijs: 2.50,
      prijsInclBtw: 5.00,
      beschrijving: 'beschrijving',
      categorie: ''
    };
    const artikel2: Artikel = {
      naam: 'FietsBel',
      id: 1,
      afbeeldingUrl: 'leuke_bel.jpg',
      beschikbaarAantal: 2,
      prijs: 2.50,
      prijsInclBtw: 5.00,
      beschrijving: 'beschrijving',
      categorie: ''
    };

    spyOn(artikelenService, 'getData').and.returnValue(
      new Observable<Artikel[]>((subject) => {
        subject.next([artikel1, artikel2]);
      })
    );

    // Act
    component.ngOnInit();

    // Assert
    expect(component.artikelen).toContain(artikel1);
    expect(component.artikelen).toContain(artikel2);
  });
});
