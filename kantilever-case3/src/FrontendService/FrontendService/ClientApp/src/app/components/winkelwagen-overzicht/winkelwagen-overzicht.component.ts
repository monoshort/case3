import { Component, OnInit, Input } from '@angular/core';
import { WinkelwagenRij } from '../../../app/models/winkelwagenrij';
import { WinkelwagenService } from '../../../app/services/winkelwagen.service';
import { systemVariables } from '../../../constants/system-variables';

@Component({
  selector: 'app-winkelwagen-overzicht',
  templateUrl: './winkelwagen-overzicht.component.html',
  styleUrls: ['./winkelwagen-overzicht.component.css']
})
export class WinkelwagenOverzichtComponent implements OnInit {

  winkelwagenRijen: WinkelwagenRij[];
  @Input() editable = false;
  verzendKosten: number = systemVariables.verzendKostenInclusiefBtw;

  constructor(private readonly winkelwagenService: WinkelwagenService) {
  }

  ngOnInit() {
    if (this.winkelwagenService.winkelwagen) {
      this.winkelwagenRijen = this.winkelwagenService.winkelwagen.artikelen;
    }
  }

  berekenRegelTotaal(regel: WinkelwagenRij): number {
    return regel.artikel.prijsInclBtw * regel.aantal;
  }

  berekenTotaal(regels: WinkelwagenRij[]): number {
    if (!regels || !regels.length) {
      return 0;
    }
    return regels.map(regel => this.berekenRegelTotaal(regel))
      .reduce((last, current) => last + current) + systemVariables.verzendKostenInclusiefBtw;
  }

  berekenExclBtwTotaal(regels: WinkelwagenRij[]): number {
    if (!regels || !regels.length) {
      return 0;
    }
    return regels.map(regel => regel.artikel.prijs * regel.aantal)
      .reduce((last, current) => last + current) + systemVariables.verzendKostenExclusiefBtw;
  }

  changeAantal(rij: WinkelwagenRij, aantal: number) {
    if (aantal < 0) {
      this.removeArtikel(rij);
    }
    this.winkelwagenService.updateArtikelAantal(rij.artikel, +aantal);
  }

  removeArtikel(rij: WinkelwagenRij) {
    this.winkelwagenRijen = this.winkelwagenRijen.filter(artikelRij => artikelRij !== rij);
    this.winkelwagenService.removeRegel(rij);
  }
}
