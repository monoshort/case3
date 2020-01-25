import { WinkelwagenService } from './winkelwagen.service';
import { Artikel } from '../models/artikel';
import { async, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';

describe('WinkelwagenService', () => {
  let service: WinkelwagenService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        ReactiveFormsModule
      ],
      declarations: [

      ],
      providers: [
        WinkelwagenService
      ]
    });
  }));

  beforeEach(() => {
    service = TestBed.get(WinkelwagenService);
  });

  it('should have an empty winkelwagen on initialization.', () => {
    // Assert
    expect(service.winkelwagen.artikelen.length).toEqual(0);
  });

  it('should add artikelen to the winkelwagen when asked to do so.', () => {
    // Arrange
    const aantalBeforeAdd = service.winkelwagen.artikelen.length;
    const artikel = { beschikbaarAantal: 10, id: 1, naam: 'Wiel', afbeeldingUrl: 'dsfasd', prijs: 12 } as Artikel;

    // Act
    service.addArtikel(artikel, 1);

    // Assert
    expect(service.winkelwagen.artikelen.length).toEqual(aantalBeforeAdd + 1);
  });

  it('should add artikel to the winkelwagen when asked to do so.', () => {
    // Arrange
    const aantalBeforeAdd = service.winkelwagen.artikelen.length;
    const artikel1 = { beschikbaarAantal: 10, id: 1, naam: 'Wiel', afbeeldingUrl: 'dsfasd', prijs: 12 } as Artikel;
    const artikel2 = { beschikbaarAantal: 21, id: 3, naam: 'Rem', afbeeldingUrl: 'dsfasd', prijs: 12 } as Artikel;
    const artikel3 = { beschikbaarAantal: 321, id: 23, naam: 'Wiel', afbeeldingUrl: 'dsfasd', prijs: 12 } as Artikel;

    // Act
    service.addArtikel(artikel1, 1);
    service.addArtikel(artikel2, 1);
    service.addArtikel(artikel3, 1);

    // Assert
    expect(service.winkelwagen.artikelen.length).toEqual(aantalBeforeAdd + 3);
  });

  it('should stack multiple artikelen in the winkelwagen when the same item is added multiple times.', () => {
    // Arrange
    const aantalBeforeAdd = service.winkelwagen.artikelen.length;
    const artikel1 = { beschikbaarAantal: 10, id: 1, naam: 'Wiel', afbeeldingUrl: 'dsfasd', prijs: 12 } as Artikel;

    // Act
    service.addArtikel(artikel1, 1);
    service.addArtikel(artikel1, 3);

    // Assert
    expect(service.winkelwagen.artikelen.length).toEqual(aantalBeforeAdd + 1);
    expect(service.winkelwagen.artikelen[0].aantal).toEqual(4);
  });

  it('should be able to update an artikel rij aantal', () => {
    // Arrange
    service.winkelwagen.artikelen = [
      {
        aantal: 3,
        artikel: {
          beschrijving: 'beschrijving',
          beschikbaarAantal: 5,
          id: 1,
          naam: 'Wiel',
          afbeeldingUrl: '',
          prijs: 16,
          prijsInclBtw: 34,
          categorie: ''
        }
      },
      {
        aantal: 2,
        artikel: {
          beschrijving: 'beschrijving',
          beschikbaarAantal: 10,
          id: 1,
          naam: 'Wiel 3',
          afbeeldingUrl: '',
          prijs: 912,
          prijsInclBtw: 73,
          categorie: ''
        }
      },
      {
        aantal: 1,
        artikel: {
          beschrijving: 'beschrijving',
          beschikbaarAantal: 8,
          id: 1,
          naam: 'Wiel rood',
          afbeeldingUrl: '',
          prijs: 312,
          prijsInclBtw: 93,
          categorie: ''
        }
      }];
    const rijToUpdate = service.winkelwagen.artikelen[0];

    // Act
    service.updateArtikelAantal(rijToUpdate.artikel, 69);

    // Assert
    const foundRij = service.winkelwagen.artikelen.find(elem => elem.artikel === rijToUpdate.artikel);
    expect(foundRij.aantal).toBe(69);

  });

  it('should be able to remove an artikel rij', () => {
    service.winkelwagen.artikelen = [
      {
        aantal: 3, artikel:
          { beschrijving: '', categorie: '', beschikbaarAantal: 5, id: 1, naam: 'wiel', afbeeldingUrl: '', prijs: 3.00, prijsInclBtw: 4.00 }
      },
      {
        aantal: 4, artikel:
        {
          beschrijving: '', categorie: '',
          beschikbaarAantal: 5, id: 2, naam: 'tandwiel',
          afbeeldingUrl: '', prijs: 3.00, prijsInclBtw: 4.00
        }
      },
      {
        aantal: 1, artikel:
          { beschrijving: '', categorie: '', beschikbaarAantal: 5, id: 3, naam: 'zit', afbeeldingUrl: '', prijs: 3.00, prijsInclBtw: 4.00 }
      },
    ];
    const rijToRemove = service.winkelwagen.artikelen[0];

    // Act
    service.removeRegel(rijToRemove);

    // Assert
    expect(service.winkelwagen.artikelen).not.toContain(rijToRemove);
  });
});
