import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BestelgegevensInvulComponent } from './bestelgegevens-invullen.component';
import { ReactiveFormsModule } from '@angular/forms';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { BestelService } from '../../services/bestel.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { AuthService } from '../../../app/services/auth.service';
import { KlantService } from '../../../app/services/klant.service';
import { AppConfigProvider } from '../../../app/providers/app-config-provider';

describe('BestelgegevensInvullenComponent', () => {
  let component: BestelgegevensInvulComponent;
  let fixture: ComponentFixture<BestelgegevensInvulComponent>;
  let authService: AuthService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule,
        ToastrModule.forRoot()
      ],
      declarations: [BestelgegevensInvulComponent],
      providers: [
        WinkelwagenService,
        AuthService,
        BestelService,
        KlantService,
        { provide: 'BASE_URL', useValue: 'http://example.com' },
        AppConfigProvider
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BestelgegevensInvulComponent);
    component = fixture.componentInstance;

    authService = TestBed.get(AuthService);
    spyOnProperty(authService, 'isLoggedIn').and.returnValue(true);

    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  // Arrange
  const postcodeValidatorTestdata = [
    { postcode: '2014XD', valid: true },
    { postcode: '2016 JP', valid: true },
    { postcode: '2014D', valid: false },
    { postcode: '201 D', valid: false },
    { postcode: '2014787FD', valid: false },
    { postcode: '1000', valid: true },
    { postcode: '1267', valid: true },
    { postcode: '12671', valid: false },
  ];

  postcodeValidatorTestdata.forEach(element => {
    it(`expects ${element.postcode} to be ${element.valid}`, () => {
      // Act
      component.postcode.setValue(element.postcode);

      // Assert
      expect(component.postcode.valid).toBe(element.valid);
    });
  });

  // Arrange
  const formValidatorTestData = [
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' }, valid: true },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' }, valid: true },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' }, valid: true },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' }, valid: true },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '', woonplaats: 'Utrecht' }, valid: false },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XPZD', woonplaats: 'Hoofddorp' }, valid: false },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: '' }, valid: false },
    { factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' }, valid: true },
  ];

  formValidatorTestData.forEach(element => {
    // Act
    it(`expects form with values: straatnaam and huisnummer ${element.factuuradres.straatnaamHuisnummer} postcode
     ${element.factuuradres.postcode} woonplaats ${element.factuuradres.woonplaats} to be ${element.valid}`, () => {
      component.straatnaamHuisnummer.setValue(element.factuuradres.straatnaamHuisnummer);
      component.postcode.setValue(element.factuuradres.postcode);
      component.woonplaats.setValue(element.factuuradres.woonplaats);

      // Assert
      expect(component.bestelgegevensForm.valid).toBe(element.valid);
    });
  });
});
