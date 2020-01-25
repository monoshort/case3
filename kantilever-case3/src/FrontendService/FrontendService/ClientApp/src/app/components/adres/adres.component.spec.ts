import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AdresComponent } from './adres.component';

describe('AdresComponent', () => {
  let component: AdresComponent;
  let fixture: ComponentFixture<AdresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AdresComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdresComponent);
    component = fixture.componentInstance;
    component.adres = { woonplaats: 'woonplaats', straatnaamHuisnummer: 'straatlaan 12', postcode: '1234 AB' };
    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should display its woonplaats', () => {
    // Arrange / Act
    const content = fixture.debugElement.nativeElement.querySelector('#woonplaats');

    // Assert
    expect(content.innerHTML).toBe('woonplaats');
  });

  it('should display its straatnaam and huisnummer', () => {
    // Arrange / Act
    const content = fixture.debugElement.nativeElement.querySelector('#straathuisnummer');

    // Assert
    expect(content.innerHTML).toBe('straatlaan 12');
  });

  it('should display its postcode', () => {
    // Arrange / Act
    const content = fixture.debugElement.nativeElement.querySelector('#postcode');

    // Assert
    expect(content.innerHTML).toBe('1234 AB');
  });

  it('should display nothing if adres isn\'t present', () => {
    // Arrange
    component.adres = undefined;

    // Act
    fixture.detectChanges();
    const content = fixture.debugElement.nativeElement.querySelector('#postcode');

    // Assert
    expect(content).toBeFalsy();
  });
});
