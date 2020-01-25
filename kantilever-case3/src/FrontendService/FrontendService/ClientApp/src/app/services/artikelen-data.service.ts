import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Artikel, ArtikelDetails } from '../models/artikel';
import { Observable } from 'rxjs';
import {endpoints} from '../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class ArtikelenDataService {
  constructor(
    private readonly http: HttpClient,
    @Inject('BASE_URL') public readonly baseUrl: string
  ) { }

  getData(): Observable<Artikel[]> {
    return this.http.get<Artikel[]>(`${this.baseUrl}${endpoints.artikelen}`);
  }

  getArtikelDetails(id: number): Observable<ArtikelDetails> {
    return this.http.get<ArtikelDetails>(`${this.baseUrl}${endpoints.artikelen}/${id}`);
  }
}
