import { Component, Input } from '@angular/core';
import { Artikel } from '../../models/artikel';
import { WinkelwagenService } from '../../services/winkelwagen.service';
import { FormGroup, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-artikel',
  templateUrl: './artikel.component.html',
  styleUrls: ['./artikel.component.css']
})
export class ArtikelComponent {
  @Input() artikel: Artikel;

  aanWinkelwagenToevoegForm = new FormGroup({
    hoeveelheid: new FormControl('1')
  });
  constructor(
    private readonly winkelwagenService: WinkelwagenService,
    private readonly _toastrService: ToastrService
  ) { }

  addToWinkelwagen() {
    this.winkelwagenService.addArtikel(this.artikel, +this.hoeveelheid);
    this._toastrService.info(`${this.hoeveelheid} x ${this.artikel.naam}`, 'Toegevoegd aan winkelwagen');
  }

  get hoeveelheid() { return this.aanWinkelwagenToevoegForm.get('hoeveelheid').value; }
}
