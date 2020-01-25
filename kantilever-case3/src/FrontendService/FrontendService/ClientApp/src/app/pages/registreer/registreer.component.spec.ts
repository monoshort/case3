import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistreerComponent } from './registreer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { WinkelwagenService } from '../../../app/services/winkelwagen.service';
import { BestelService } from '../../../app/services/bestel.service';
import { AppConfigProvider } from '../../../app/providers/app-config-provider';

describe('RegistreerComponent', () => {
  let component: RegistreerComponent;
  let fixture: ComponentFixture<RegistreerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, HttpClientTestingModule, RouterTestingModule, ToastrModule.forRoot()],
      declarations: [RegistreerComponent],
      providers: [WinkelwagenService, BestelService, { provide: 'BASE_URL', useValue: 'http://example.com' }, AppConfigProvider]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistreerComponent);
    component = fixture.componentInstance;
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
    {
      telefoonnummer: '0612345678',
      naam: 'Steven Kazan',
      email: 'steven.k@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: true
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Jan willem de vries',
      email: 'janwillen.dv@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: true
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Steven Kazan-Carrera',
      email: 'steven.kc@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: true
    },
    {
      telefoonnummer: '0612345678',
      naam: '',
      email: 'bram.donk@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Steven Kazan',
      email: 'steven.k@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: '', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Sjaak Trekhaak',
      email: 'sjaak.t@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: true
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Tjerk Dobberman',
      email: 'tjerk.d@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '', woonplaats: 'Utrecht' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Aart Larie',
      email: 'aart.l@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XPZD', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '',
      naam: 'Aart Laurier',
      email: 'aart.l@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011ZD', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Tjerk Dobberman',
      email: 'tjerk.d@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: '' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Hans Kazan',
      email: 'hans.k@mail.com',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: true
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Willem Kazan',
      email: 'willem.k@mail.com',
      pass1: 'password',
      pass2: '',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Hendrik Kazan',
      email: 'hendrik.k@mail.com',
      pass1: '',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Paul Kazan',
      email: '',
      pass1: 'password',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: false
    },
    {
      telefoonnummer: '0612345678',
      naam: 'Freek Kazan',
      email: 'freek.k@mail.com',
      pass1: 'pass',
      pass2: 'password',
      factuuradres: { straatnaamHuisnummer: 'Herenplantsoen 2', postcode: '2011XP', woonplaats: 'Hoofddorp' },
      valid: false
    }
  ];

  formValidatorTestData.forEach(element => {
    // Act
    it(`expects form with values: naam ${element.naam} straatnaam ${element.factuuradres.straatnaamHuisnummer} postcode
     ${element.factuuradres.postcode} woonplaats ${element.factuuradres.woonplaats} to be ${element.valid}`, () => {
      component.naam.setValue(element.naam);
      component.telefoonnummer.setValue(element.telefoonnummer);
      component.straatnaamHuisnummer.setValue(element.factuuradres.straatnaamHuisnummer);
      component.postcode.setValue(element.factuuradres.postcode);
      component.woonplaats.setValue(element.factuuradres.woonplaats);
      component.email.setValue(element.email);
      component.password.setValue(element.pass1);
      component.password2.setValue(element.pass2);

      // Assert
      expect(component.formsAreValid).toBe(element.valid);
    });
  });

});
