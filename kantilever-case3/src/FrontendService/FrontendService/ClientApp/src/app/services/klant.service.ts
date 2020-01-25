import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Klant } from '../../app/models/klant';
import { endpoints } from '../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class KlantService {

  constructor(
    private readonly _http: HttpClient,
    @Inject('BASE_URL') public readonly baseUrl: string) {
  }

  public getKlant(username: string): Observable<Klant> {
    return this._http.get<Klant>(`${this.baseUrl}${endpoints.klanten}/${encodeURIComponent(username)}`);
  }

}
