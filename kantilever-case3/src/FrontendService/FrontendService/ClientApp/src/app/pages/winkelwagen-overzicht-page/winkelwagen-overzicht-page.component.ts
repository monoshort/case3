import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { WinkelwagenService } from '../../services/winkelwagen.service';

@Component({
  selector: 'app-winkelwagen-overzicht-page',
  templateUrl: './winkelwagen-overzicht-page.component.html',
  styleUrls: ['./winkelwagen-overzicht-page.component.css']
})
export class WinkelwagenOverzichtPageComponent {

  constructor(
    private readonly _router: Router,
    private readonly _winkelwagenService: WinkelwagenService) {
  }

  bestel() {
    this._router.navigate(['bestelgegevens-invoeren']);
  }

  winkelwagenHasArtikelen() {
    return this._winkelwagenService.winkelwagen.artikelen.length !== 0;
  }
}
