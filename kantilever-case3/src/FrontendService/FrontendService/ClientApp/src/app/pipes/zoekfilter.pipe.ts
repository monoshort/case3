import { Pipe, PipeTransform } from '@angular/core';
import { Artikel } from '../models/artikel';

@Pipe({
  name: 'zoekfilter'
})
export class ZoekfilterPipe implements PipeTransform {

  transform(items: Artikel[], searchText: string): Artikel[] {
    if (!items) {
      return [];
    }
    if (searchText === '') {
      return items;
    }
    searchText = searchText.toLowerCase().trim();
    return items.filter(artikel => {
      return (`${artikel.naam.toLowerCase()}
      ${artikel.beschrijving.toLowerCase()}
      ${artikel.categorie.toLowerCase()}`.includes(searchText));
    });
  }
}
