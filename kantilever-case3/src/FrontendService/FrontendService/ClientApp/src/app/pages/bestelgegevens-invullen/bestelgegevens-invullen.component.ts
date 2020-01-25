import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { WinkelwagenService } from '../../../app/services/winkelwagen.service';
import { Klant } from '../../../app/models/klant';
import { Adres } from '../../../app/models/adres';
import { Router } from '@angular/router';
import { BestelService } from '../../../app/services/bestel.service';
import { KlantService } from '../../../app/services/klant.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../app/services/auth.service';

@Component({
  selector: 'app-bestelgegevens-invullen',
  templateUrl: './bestelgegevens-invullen.component.html',
  styleUrls: ['./bestelgegevens-invullen.component.css']
})
export class BestelgegevensInvulComponent implements OnInit {

  public _klant: Klant;

  public bestelgegevensForm: FormGroup = new FormGroup({
    straatnaamHuisnummer: new FormControl('', Validators.required),
    postcode: new FormControl('', [Validators.pattern(/^[1-9][0-9]{3} ?(?!sa|sd|ss)([a-z]{2})?$/i), Validators.required]),
    woonplaats: new FormControl('', Validators.required)
  });

  get straatnaamHuisnummer() {
    return this.bestelgegevensForm.get('straatnaamHuisnummer');
  }

  get postcode() {
    return this.bestelgegevensForm.get('postcode');
  }

  get woonplaats() {
    return this.bestelgegevensForm.get('woonplaats');
  }

  constructor(
    private readonly _router: Router,
    private readonly _winkelwagenService: WinkelwagenService,
    private readonly _bestelService: BestelService,
    private readonly _klantService: KlantService,
    private readonly _toastrService: ToastrService,
    private readonly _authService: AuthService
  ) { }

  ngOnInit() {
    this._getKlant();
  }
  private _autoFillForm(adres: Adres) {
    this.bestelgegevensForm.get('straatnaamHuisnummer').setValue(adres.straatnaamHuisnummer);
    this.bestelgegevensForm.get('postcode').setValue(adres.postcode);
    this.bestelgegevensForm.get('woonplaats').setValue(adres.woonplaats);
  }

  private _getKlant() {
    if (!this._authService.isLoggedIn) {
      this._authService.login();
      return;
    }
    this._klantService.getKlant(this._authService.username).subscribe(
      (klant: Klant) => {
        this._klant = klant;
        this._autoFillForm(klant.factuuradres);
      });
  }

  public bestel() {
    if (this._klant && this.bestelgegevensForm.valid) {
      const afleverAdres: Adres = {
        straatnaamHuisnummer: this.straatnaamHuisnummer.value,
        postcode: this.postcode.value,
        woonplaats: this.woonplaats.value
      };

      this._bestelService.bestelling = {
        klant: this._klant,
        winkelwagen: this._winkelwagenService.winkelwagen,
        afleverAdres
      };

      this._router.navigate(['bestel-overzicht']);
    } else if (!this.bestelgegevensForm.valid) {
      this._toastrService.error('Controleer het formulier.');
    } else {
      this._toastrService.error('Er is iets misgegaan!');
    }

  }
}
