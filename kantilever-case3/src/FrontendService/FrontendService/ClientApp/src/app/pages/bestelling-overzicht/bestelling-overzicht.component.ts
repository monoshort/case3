import { Component, OnInit } from '@angular/core';
import { BestelService } from '../../services/bestel.service';
import { Adres } from '../../models/adres';
import { Bestelling } from '../../models/bestelling';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { WinkelwagenService } from '../../services/winkelwagen.service';

@Component({
  selector: 'app-bestelling-overzicht',
  templateUrl: './bestelling-overzicht.component.html',
  styleUrls: ['./bestelling-overzicht.component.css']
})
export class BestellingOverzichtComponent implements OnInit {

  public factuurAdres: Adres;
  public afleverAdres: Adres;
  public bestelling: Bestelling;

  constructor(
    private readonly _bestelService: BestelService,
    private readonly _winkelwagenService: WinkelwagenService,
    private readonly _router: Router,
    private readonly _toastr: ToastrService
  ) { }

  ngOnInit() {
    this.bestelling = this._bestelService.bestelling;
    if (this.bestelling && this.bestelling.klant) {
      this.factuurAdres = this.bestelling.klant.factuuradres;
      this.afleverAdres = this.bestelling.afleverAdres;
    } else {
      this._router.navigate(['']);
    }
  }

  bestellen() {
    this._bestelService.bestel(this.bestelling).subscribe(
      data => {
        this._router.navigate(['']);
        this._bestelService.bestelling = undefined;
        this._winkelwagenService.winkelwagen.artikelen = [];
        this._toastr.success('Bestelling succesvol geplaatst!');
      },
      error => {
        this._toastr.error('Er is iets fout gegaan');
      }
    );
  }

  terug() {
    window.history.go(-1);
  }
}
