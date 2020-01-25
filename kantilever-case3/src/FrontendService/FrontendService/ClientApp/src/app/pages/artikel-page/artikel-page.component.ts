import { Component, OnInit } from '@angular/core';
import { ArtikelenDataService } from '../../services/artikelen-data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ArtikelDetails } from '../../models/artikel';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormControl } from '@angular/forms';
import { WinkelwagenService } from '../../services/winkelwagen.service';

@Component({
  selector: 'app-artikel-page',
  templateUrl: './artikel-page.component.html',
  styleUrls: ['./artikel-page.component.css']
})
export class ArtikelPageComponent implements OnInit {

  artikel: ArtikelDetails;

  aanWinkelwagenToevoegForm = new FormGroup({
    hoeveelheid: new FormControl('1')
  });

  constructor(
    private readonly _artikelService: ArtikelenDataService,
    private readonly _winkelwagenService: WinkelwagenService,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _toasterService: ToastrService,
    private readonly _router: Router
  ) { }

  ngOnInit() {
    const { artikelId } = this._activatedRoute.snapshot.params;
    this._artikelService.getArtikelDetails(artikelId).subscribe(
      artikel => {
        this.artikel = artikel;
      }, error => {
        this._router.navigate(['']);
        this._toasterService.error('Artikel niet gevonden.', 'Niet gevonden');
      });
  }

  get hoeveelheid() { return this.aanWinkelwagenToevoegForm.get('hoeveelheid').value; }

  addToWinkelwagen() {
    this._winkelwagenService.addArtikel(this.artikel, +this.hoeveelheid);
    this._toasterService.info(`${this.hoeveelheid} x ${this.artikel.naam}`, 'Toegevoegd aan winkelwagen');
  }

}
