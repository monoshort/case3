import {Adres} from './adres';

export interface Klant {
    id: number;
    naam: string;
    factuuradres: Adres;
    telefoonnummer: string;
}
