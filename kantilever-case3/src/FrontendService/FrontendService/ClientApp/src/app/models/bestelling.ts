import { Winkelwagen } from './winkelwagen';
import { Klant } from './klant';
import { Adres } from '../../app/models/adres';

export interface Bestelling {
    winkelwagen: Winkelwagen;
    klant: Klant;
    afleverAdres: Adres;
}
