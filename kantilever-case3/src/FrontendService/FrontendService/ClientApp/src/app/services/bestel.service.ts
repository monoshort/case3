import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Bestelling } from '../models/bestelling';
import { Observable } from 'rxjs';
import { BestellingResult } from '../models/bestellingresult';
import { GemaakteBestelling } from '../models/gemaakte-bestelling';
import { ToastrService } from 'ngx-toastr';
import {endpoints} from '../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class BestelService {

  public bestelling: Bestelling;
  constructor(
    private readonly httpClient: HttpClient,
    @Inject('BASE_URL') private readonly baseUrl: string,
    private readonly toastrService: ToastrService
  ) { }

  bestel(bestelling: Bestelling): Observable<BestellingResult> {
    return this.httpClient.post<BestellingResult>(`${this.baseUrl}${endpoints.bestellingen}`, bestelling);
  }

  public alleBestellingenVanKlant(username: string): Observable<GemaakteBestelling[]> {
    return this.httpClient.get<GemaakteBestelling[]>(`${this.baseUrl}${endpoints.klantBestellingen}/${encodeURIComponent(username)}`, {});
  }

  laatMeldingZienVanNietAutomatischGoedgekeurdeBestellingen(username: string) {
    this.alleBestellingenVanKlant(username).subscribe(
      res => {
        this.checkBestellingenVoorNietGoedgekeurd(res);
      }
    );
  }

  checkBestellingenVoorNietGoedgekeurd(bestellingen: GemaakteBestelling[]) {
    const containsNietGoedgekeurdeBestellingen = bestellingen.some(element => !element.goedgekeurd);

    if (containsNietGoedgekeurdeBestellingen) {
      this.toastrService.warning('Een bestelling is niet automatisch goedgekeurd.', 'Waarschuwing:', {
        tapToDismiss: true,
        disableTimeOut: true
      });
    }
  }
}

