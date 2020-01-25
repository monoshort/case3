import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../app/services/auth.service';
import { RegistreerModel } from '../../../app/models/registreerModel';
import { ToastrService } from 'ngx-toastr';
import { Adres } from '../../../app/models/adres';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-registreer',
  templateUrl: './registreer.component.html',
  styleUrls: ['./registreer.component.css']
})
export class RegistreerComponent implements OnInit {

  public registreerForm: FormGroup;

  get email() {
    return this.registreerForm.get('email');
  }

  get password() {
    return this.registreerForm.get('password');
  }

  get password2() {
    return this.registreerForm.get('password2');
  }

  get naam() {
    return this.registreerForm.get('naam');
  }

  get telefoonnummer() {
    return this.registreerForm.get('telefoonnummer');
  }

  get straatnaamHuisnummer() {
    return this.registreerForm.get('adres.straatnaamHuisnummer');
  }

  get postcode() {
    return this.registreerForm.get('adres.postcode');
  }

  get woonplaats() {
    return this.registreerForm.get('adres.woonplaats');
  }

  get formsAreValid() {
    return this.registreerForm.valid && this.passwordsAreEqual;
  }

  get passwordsAreEqual(): boolean {
    return this.password.value === this.password2.value;
  }

  constructor(public readonly _authService: AuthService,
    private readonly _formBuilder: FormBuilder,
    private readonly _toastrService: ToastrService) {
  }

  ngOnInit() {
    this._buildForm();
  }

  public _buildForm(): void {
    this.registreerForm = this._formBuilder.group({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      password2: new FormControl('', Validators.required),
      naam: new FormControl('', Validators.required),
      telefoonnummer: new FormControl('', Validators.required),
      adres: this._formBuilder.group({
        straatnaamHuisnummer: new FormControl('', Validators.required),
        postcode: new FormControl('', [Validators.pattern(/^[1-9][0-9]{3} ?(?!sa|sd|ss)([a-z]{2})?$/i), Validators.required]),
        woonplaats: new FormControl('', Validators.required)
      })
    });
  }

  public registreer() {
    if (!this.formsAreValid) {
      this._toastrService.error('Er ging iets mis, controleer de velden van het formulier.');
      return;
    }

    const adres: Adres = {
      straatnaamHuisnummer: this.straatnaamHuisnummer.value,
      postcode: this.postcode.value,
      woonplaats: this.woonplaats.value
    };

    const result: RegistreerModel = {
      username: this.email.value,
      password: this.password.value,
      naam: this.naam.value,
      telefoonnummer: this.telefoonnummer.value,
      adres
    };

    this._authService.registreer(result)
      .subscribe((data: HttpResponse<any>) => {
        this._registerSuccesToast();
        this._authService.login();
      },
        (error: HttpErrorResponse) => {
          this._registerErrorToast();
        }
      );

  }

  private _registerSuccesToast(): void {
    this._toastrService.success('Registratie succesvol!');
  }

  private _registerErrorToast() {
    this._toastrService.error('Er ging iets fout bij het registreren!');
  }

}
