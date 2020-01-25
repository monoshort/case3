import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtikelComponent } from './artikel.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { Artikel } from '../../models/artikel';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ArtikelComponent', () => {
  let component: ArtikelComponent;
  let fixture: ComponentFixture<ArtikelComponent>;
  let winkelwagenService: WinkelwagenService;
  let toastrService: ToastrService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        ReactiveFormsModule,
        ToastrModule.forRoot(),
        BrowserAnimationsModule,
        RouterTestingModule,
        AngularFontAwesomeModule,
        FormsModule
      ],
      declarations: [ArtikelComponent],
      providers: [
        WinkelwagenService,
        ToastrService,
        AngularFontAwesomeModule,
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArtikelComponent);
    component = fixture.componentInstance;
    component.artikel = {
      beschikbaarAantal: 4,
      naam: 'artikelNaam',
      prijs: 10.00,
      id: 99,
      afbeeldingUrl: '',
      prijsInclBtw: 9.00,
      beschrijving: '',
      categorie: ''
    };
    winkelwagenService = TestBed.get(WinkelwagenService);
    toastrService = TestBed.get(ToastrService);

    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should display its artikel naam', () => {
    // Arrange / Act
    const content = fixture.debugElement.nativeElement.querySelector('#naam');

    // Assert
    expect(content.innerHTML).toBe('artikelNaam');
  });

  it('should display its artikel prijs Incl Btw', () => {
    // Arrange / Act
    const content = fixture.debugElement.nativeElement.querySelector('#price');

    // Assert
    expect(content.innerHTML).toBe('€9.00');
  });

  it('should display its artikel amount if its less than 8', () => {
    // Arrange
    component.artikel.beschikbaarAantal = 4;

    // Act
    fixture.detectChanges();
    const content = fixture.debugElement.nativeElement.querySelector('#amount');

    // Assert
    expect(content.innerHTML).toContain('4');
  });

  it('should display its artikel amount as 8 if its more than 8', () => {
    // Arrange
    component.artikel.beschikbaarAantal = 99;

    // Act
    fixture.detectChanges();
    const content = fixture.debugElement.nativeElement.querySelector('#amount');

    // Assert
    expect(content.innerHTML).toContain('8');
  });

  it('should call the WinkelwagenService.add with a "bike" and 12 as parameters.', () => {
    // Arrange
    spyOn(winkelwagenService, 'addArtikel');
    component.aanWinkelwagenToevoegForm.get('hoeveelheid').setValue('12');
    component.artikel = { naam: 'bike' } as Artikel;

    // Act
    component.addToWinkelwagen();

    // Assert
    expect(winkelwagenService.addArtikel).toHaveBeenCalledWith({ naam: 'bike' } as Artikel, 12);
  });

  it('should call the toastrservice with the article and amount that was added to basket', () => {
    // Arrange
    spyOn(toastrService, 'info');
    component.aanWinkelwagenToevoegForm.get('hoeveelheid').setValue('18');
    component.artikel = { naam: 'fiets' } as Artikel;

    // Act
    component.addToWinkelwagen();

    // Assert
    expect(toastrService.info).toHaveBeenCalledWith('18 x fiets', 'Toegevoegd aan winkelwagen');
  });
});
