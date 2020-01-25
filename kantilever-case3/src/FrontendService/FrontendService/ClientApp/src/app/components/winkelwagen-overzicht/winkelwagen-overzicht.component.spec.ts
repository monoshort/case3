import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WinkelwagenOverzichtComponent } from './winkelwagen-overzicht.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WinkelwagenRij } from '../../models/winkelwagenrij';
import {systemVariables} from '../../../constants/system-variables';

describe('WinkelwagenOverzichtComponent', () => {
  let component: WinkelwagenOverzichtComponent;
  let fixture: ComponentFixture<WinkelwagenOverzichtComponent>;
  let winkelwagenService: WinkelwagenService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        WinkelwagenOverzichtComponent
      ],
      imports: [
        RouterTestingModule,
        ToastrModule.forRoot(),
        HttpClientTestingModule,
        BrowserAnimationsModule,
        AngularFontAwesomeModule
      ],
      providers: [
        WinkelwagenService,
        { provide: 'BASE_URL', useValue: '' }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WinkelwagenOverzichtComponent);
    component = fixture.componentInstance;
    winkelwagenService = TestBed.get(WinkelwagenService);
    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('Sets verzendkosten inclusief btw properly', () => {
    // Assert
    expect(component.verzendKosten).toEqual(systemVariables.verzendKostenInclusiefBtw);
  });

  const lineTotalsTestData = [
    {
      artikel: {
        afbeeldingUrl: '',
        beschikbaarAantal: 2,
        naam: 'een product',
        id: 1,
        prijs: 0,
        prijsInclBtw: 9.00,
        beschrijving: '',
        categorie: ''
      },
      aantal: 2, expected: 18
    },
    {
      artikel: {
        afbeeldingUrl: '',
        beschikbaarAantal: 2,
        naam: 'een product',
        id: 1,
        prijs: 0,
        prijsInclBtw: 4.50,
        beschrijving: '',
        categorie: ''
      },
      aantal: 4, expected: 18
    },
    {
      artikel: {
        afbeeldingUrl: '',
        beschikbaarAantal: 2,
        naam: 'een product',
        id: 1,
        prijs: 0,
        prijsInclBtw: 2.25,
        beschrijving: '',
        categorie: ''
      },
      aantal: 6, expected: 13.5
    },
  ];

  lineTotalsTestData.forEach(rij => {
    it('should properly calculate line totals incl. btw', () => {
      // Act
      const result = component.berekenRegelTotaal(rij);

      // Assert
      expect(result).toBe(rij.expected);
    });
  });

  const totalTestData = [
    {
      data: [
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 2,
            naam: 'een product',
            id: 1,
            prijs: 0,
            prijsInclBtw: 9,
            beschrijving: '',
            categorie: ''
          },
          aantal: 2
        },
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 3,
            naam: 'een ander product',
            id: 2,
            prijs: 0,
            prijsInclBtw: 9,
            beschrijving: '',
            categorie: ''
          },
          aantal: 2
        },
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 8,
            naam: 'een goedkoop product',
            id: 3,
            prijs: 0,
            prijsInclBtw: 9,
            beschrijving: '',
            categorie: ''
          },
          aantal: 1
        },
      ],
      expected: 51
    },
    {
      data: [
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 2,
            naam: 'een product',
            id: 1,
            prijs: 0,
            prijsInclBtw: 30,
            beschrijving: '',
            categorie: ''
          },
          aantal: 1
        },
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 3,
            naam: 'een ander product',
            id: 2,
            prijs: 0,
            prijsInclBtw: 15,
            beschrijving: '',
            categorie: ''
          },
          aantal: 2
        },
      ],
      expected: 66
    },
    {
      data: [
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 2,
            naam: 'een product',
            id: 1,
            prijs: 0,
            prijsInclBtw: 2.50,
            beschrijving: '',
            categorie: ''
          },
          aantal: 4
        },
        {
          artikel: {
            afbeeldingUrl: '',
            beschikbaarAantal: 3,
            naam: 'een ander product',
            id: 2,
            prijs: 0,
            prijsInclBtw: 2,
            beschrijving: '',
            categorie: ''
          },
          aantal: 2
        },
      ],
      expected: 20
    },
  ];

  totalTestData.forEach(testData => {
    it('should properly calculate order total incl. btw and ', () => {
      // Act
      const result = component.berekenTotaal(testData.data);

      // Assert
      expect(result).toBe(testData.expected);
    });
  });

  it('should add verzend kosten to totaal in berekenTotaal', () => {
    // Arrange
    const totaal = [
      {
        artikel: {
          afbeeldingUrl: '',
          beschikbaarAantal: 2,
          naam: 'een product',
          id: 1,
          prijs: 0,
          prijsInclBtw: 5.00,
          beschrijving: '',
          categorie: ''
        },
        aantal: 1
      },
    ];

    // Act
    const result = component.berekenTotaal(totaal);

    // Assert
    expect(result).toBe(11.00);
  });

  it('should add verzend kosten to totaal in berekenExclBtwTotaal', () => {
    // Arrange
    const totaal = [
      {
        artikel: {
          afbeeldingUrl: '',
          beschikbaarAantal: 2,
          naam: 'een product',
          id: 1,
          prijs: 5.00,
          prijsInclBtw: 0,
          beschrijving: '',
          categorie: ''
        },
        aantal: 1
      },
    ];

    // Act
    const result = component.berekenExclBtwTotaal(totaal);

    // Assert
    expect(result).toBe(5.00 + systemVariables.verzendKostenExclusiefBtw);
  });

  it('should display a message if there are no artikelen to display', () => {
    // Arrange
    component.winkelwagenRijen = undefined;

    // Act
    fixture.detectChanges();
    const content = fixture.debugElement.nativeElement.querySelector('#geenArtikelen');

    // Assert
    expect(content.innerHTML).toContain('winkelmandje is leeg, ga snel op de artikelen pagina kijken');
  });

  it('should be able to change a regel in the winkelwagen', () => {
    // Arrange
    const testdata = totalTestData[0].data;
    const regelToChange = testdata[0];
    spyOn(winkelwagenService, 'updateArtikelAantal');

    // Act
    component.changeAantal(regelToChange, 3 * regelToChange.aantal);

    // Assert
    expect(winkelwagenService.updateArtikelAantal).toHaveBeenCalled();
  });

  it('should be able to change a regel in the winkelwagen but not be set to something lower than 0, else deleting the regel', () => {
    // Arrange
    const testdata = totalTestData[0].data;
    const regelToRemove = testdata[0];
    spyOn(winkelwagenService, 'removeRegel');

    // Act
    component.changeAantal(regelToRemove, -20);

    // Assert
    expect(winkelwagenService.removeRegel).toHaveBeenCalledWith(regelToRemove);
  });

  it('should be able to remove a regel in the winkelwagen', () => {
    const regelToRemove: WinkelwagenRij = {
      artikel: {
        afbeeldingUrl: '',
        beschikbaarAantal: 3,
        naam: 'een ander product',
        id: 2,
        prijs: 0,
        prijsInclBtw: 9,
        beschrijving: '',
        categorie: ''
      },
      aantal: 2
    };
    component.removeArtikel(regelToRemove);

    expect(component.winkelwagenRijen).not.toContain(regelToRemove);
  });
});
