import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WinkelwagenOverzichtPageComponent } from './winkelwagen-overzicht-page.component';
import { WinkelwagenOverzichtComponent } from '../../components/winkelwagen-overzicht/winkelwagen-overzicht.component';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Artikel } from '../../models/artikel';
import { AngularFontAwesomeModule } from 'angular-font-awesome';

describe('WinkelwagenOverzichtPageComponent', () => {
  let component: WinkelwagenOverzichtPageComponent;
  let fixture: ComponentFixture<WinkelwagenOverzichtPageComponent>;
  let winkelwagenService: WinkelwagenService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        WinkelwagenOverzichtPageComponent,
        WinkelwagenOverzichtComponent
      ],
      providers: [
        WinkelwagenService,
      ],
      imports: [
        RouterTestingModule,
        AngularFontAwesomeModule
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WinkelwagenOverzichtPageComponent);
    component = fixture.componentInstance;
    winkelwagenService = TestBed.get(WinkelwagenService);
    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should know when the there arn\'t any artikelen to show', () => {
    // Arrange
    winkelwagenService.winkelwagen.artikelen = [];
    fixture.detectChanges();

    // Act
    const result = component.winkelwagenHasArtikelen();

    // Assert
    expect(result).toBeFalsy();
  });

  it('shouldn\'t show the bestel button if there aren\'t any artikelen to show ', () => {
    // Arrange
    winkelwagenService.winkelwagen.artikelen = [];
    fixture.detectChanges();

    // Act
    const content = fixture.debugElement.nativeElement.querySelector('#bestelButton');

    // Assert
    expect(content).toBeFalsy();
  });

  it('should show the bestel button if there are artikelen to show ', () => {
    // Arrange
    const artikel: Artikel = {
      naam: 'Fiets',
      id: 1,
      afbeeldingUrl: 'leuke_fiets.jpg',
      beschikbaarAantal: 6,
      prijs: 2.50,
      prijsInclBtw: 5.00,
      beschrijving: 'beschrijving',
      categorie: ''
    };
    winkelwagenService.winkelwagen.artikelen = [{ aantal: 2, artikel }];
    fixture.detectChanges();

    // Act
    const content = fixture.debugElement.nativeElement.querySelector('#bestelButton');

    // Assert
    expect(content).toBeTruthy();
  });
});
