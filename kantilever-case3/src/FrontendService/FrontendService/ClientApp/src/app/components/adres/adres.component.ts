import { Component, Input } from '@angular/core';
import { Adres } from '../../../app/models/adres';

@Component({
  selector: 'app-adres',
  templateUrl: './adres.component.html',
  styleUrls: ['./adres.component.css']
})
export class AdresComponent {
  @Input() adres: Adres;
}
