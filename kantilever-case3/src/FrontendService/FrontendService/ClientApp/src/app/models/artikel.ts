export interface Artikel {
    id: number;
    naam: string;
    prijs: number;
    prijsInclBtw: number;
    beschikbaarAantal: number;
    afbeeldingUrl: string;
    beschrijving: string;
    categorie: string;
}
export interface ArtikelDetails {
    id: number;
    naam: string;
    prijs: number;
    prijsInclBtw: number;
    beschikbaarAantal: number;
    afbeeldingUrl: string;
    beschrijving: string;
    leverbaarVanaf: Date;
    leverbaarTot: Date;
    leverancierCode: string;
    leverancier: string;
    categorie: string;
    subCategorie: string;
}
