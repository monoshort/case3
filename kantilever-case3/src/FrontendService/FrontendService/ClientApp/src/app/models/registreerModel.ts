import { Adres } from '../../app/models/adres';

export interface RegistreerModel {
  username: string;
  password: string;
  naam: string;
  telefoonnummer: string;
  adres: Adres;
}
