import { ZoekfilterPipe } from './zoekfilter.pipe';
import { Artikel } from '../models/artikel';

describe('ZoekfilterPipe', () => {
  it('create an instance', () => {
    // Arrange
    const pipe = new ZoekfilterPipe();

    // Assert
    expect(pipe).toBeTruthy();
  });

  it('should return only items which have the searched-for word in beschrijving of naam', () => {
    // Arrange
    const testParameters = [
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'banaan',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'fietskettip',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'gnee',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'blablabla',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'jaap', beschrijving: 'fietsketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'ja',
        deelVanResultaat: true
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'jaap', categorie: '' }
          ] as Artikel[],
        zoekterm: 'ja',
        deelVanResultaat: true
      },
    ];
    const pipe = new ZoekfilterPipe();

    // Act
    testParameters.forEach(element => {
      const result = pipe.transform(element.artikelen, element.zoekterm);

      // Assert
      if (element.deelVanResultaat) {
        expect(result).toContain(element.artikelen[0]);
      } else {
        expect(result).not.toContain(element.artikelen[0]);
      }
    });
  });

  it('should return only items which have the searched-for word in beschrijving of naam and spaces dont get included from zoekterm', () => {
    // Arrange
    const testParameters = [
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'banaan',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'fietskettip',
        deelVanResultaat: false
      },
      {
        artikelen: [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
        ] as Artikel[],
        zoekterm: 'gnee',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'blablabla',
        deelVanResultaat: false
      },
      {
        artikelen:
          [{ naam: 'jaap', beschrijving: 'fietsketting', categorie: '' }
          ] as Artikel[],
        zoekterm: 'ja           ',
        deelVanResultaat: true
      },
      {
        artikelen:
          [{ naam: 'fietsketting', beschrijving: 'jaap', categorie: '' }
          ] as Artikel[],
        zoekterm: '           ja',
        deelVanResultaat: true
      },
    ];
    const pipe = new ZoekfilterPipe();

    // Act
    testParameters.forEach(element => {
      const result = pipe.transform(element.artikelen, element.zoekterm);

      // Assert
      if (element.deelVanResultaat) {
        expect(result).toContain(element.artikelen[0]);
      } else {
        expect(result).not.toContain(element.artikelen[0]);
      }
    });
  });

  it('should return all items if searchbar is empty', () => {
    // Arrange
    const zoekterm = '';
    const pipe = new ZoekfilterPipe();
    const artikelen =
      [
        { naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' } as Artikel,
        { naam: 'fietsbel', beschrijving: 'belbel', categorie: '' } as Artikel,
        { naam: 'band', beschrijving: 'wielband', categorie: '' } as Artikel,
      ] as Artikel[];
    // Act

    const result = pipe.transform(artikelen, zoekterm);

    // Assert
    expect(result.length).toBe(3);
    expect(result).toContain({ naam: 'fietsketting', beschrijving: 'geenketting', categorie: '' } as Artikel);
    expect(result).toContain({ naam: 'fietsbel', beschrijving: 'belbel', categorie: '' } as Artikel);
    expect(result).toContain({ naam: 'band', beschrijving: 'wielband', categorie: '' } as Artikel);
  });

  it('should return only items which have the searched-for word in beschrijving, naam, \
  or categorie and spaces dont get included from zoekterm', () => {
    // Arrange
    const testParameters = [
      {
        artikelen:
          [
            { naam: 'fietsketting', beschrijving: 'beschrijving', categorie: 'jasbeschermer' }
          ] as Artikel[],
        zoekterm: '           ja',
        deelVanResultaat: true
      },
      {
        artikelen:
          [
            { naam: 'fietsketting', beschrijving: 'fsaaskjhf', categorie: 'aj aj aj' }
          ] as Artikel[],
        zoekterm: '           ja',
        deelVanResultaat: false
      },
    ];
    const pipe = new ZoekfilterPipe();

    // Act
    testParameters.forEach(element => {
      const result = pipe.transform(element.artikelen, element.zoekterm);

      // Assert
      if (element.deelVanResultaat) {
        expect(result).toContain(element.artikelen[0]);
      } else {
        expect(result).not.toContain(element.artikelen[0]);
      }
    });
  });
});
