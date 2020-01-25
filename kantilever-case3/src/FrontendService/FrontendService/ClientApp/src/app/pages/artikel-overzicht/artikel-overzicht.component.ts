import { Component, OnInit } from '@angular/core';
import { Artikel } from '../../models/artikel';
import { ArtikelenDataService } from '../../services/artikelen-data.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-artikel-overzicht',
  templateUrl: './artikel-overzicht.component.html',
  styleUrls: ['./artikel-overzicht.component.css']
})
export class ArtikelOverzichtComponent implements OnInit {
  artikelen: Artikel[] = [];
  searchText = '';
  constructor(
    private readonly _artikelenDataService: ArtikelenDataService,
    private readonly _activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this._artikelenDataService.getData().subscribe(artikelen => {
      this.artikelen = artikelen;
    });
  }
}
