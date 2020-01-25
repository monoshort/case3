import { Injectable } from '@angular/core';
import { Winkelwagen } from '../models/winkelwagen';
import { Artikel } from '../models/artikel';
import { WinkelwagenRij } from '../models/winkelwagenrij';

@Injectable({
  providedIn: 'root'
})
export class WinkelwagenService {
  readonly winkelwagen: Winkelwagen;

  constructor() {
    this.winkelwagen = {} as Winkelwagen;
    this.winkelwagen.artikelen = [];
  }

  addArtikel(artikel: Artikel, aantal: number) {
    const artikelInWinkelwagen = this._findArtikelInWinkelwagen(artikel);
    if (artikelInWinkelwagen) {
      artikelInWinkelwagen.aantal += aantal;
    } else {
      this.winkelwagen.artikelen.push({ artikel, aantal } as WinkelwagenRij);
    }
  }
  updateArtikelAantal(artikel: Artikel, aantal: number) {
    const artikelInWinkelwagen = this._findArtikelInWinkelwagen(artikel);
    if (artikelInWinkelwagen) {
      artikelInWinkelwagen.aantal = aantal;
    }
  }

  removeRegel(rij: WinkelwagenRij) {
    this.winkelwagen.artikelen = this.winkelwagen.artikelen.filter(artikelRij => artikelRij !== rij);
  }

  private _findArtikelInWinkelwagen(artikel: Artikel): WinkelwagenRij {
    return this.winkelwagen.artikelen.find(x => x.artikel === artikel);
  }
}

