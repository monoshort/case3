using System;
using System.Collections.Generic;
using System.Linq;
using BestelService.Core.Events;
using BestelService.Core.Exceptions;

namespace BestelService.Core.Models
{
    public class Bestelling
    {
        internal const string BestelRegelsZijnNogNietIngepaktMessage = "Nog niet alle bestelregels zijn ingepakt";
        internal const string FactuurIsNietUigeprintMessage = "Factuur is nog niet geprint";
        internal const string AdresLabelIsNietUigeprintMessage = "AdresLabel is nog niet geprint";
        internal const string OpenstaandBedragNietNegatief = "Openstaand bedrag kan niet negatief zijn";

        internal const int DagenOmTeBetalen = 31;
        internal const decimal AutomatischGoedgekeurdMaximalePrijs = 500M;
        internal const int BestelNummerStartNumber = 10000;

        public event BestellingKanKlaargemeldWordenEventHandler BestellingKanKlaargemeldWorden;
        public event BestellingKlantIsWanBetalerGewordenEventHandler BestellingKlantIsWanbetalerGeworden;

        public long Id { get; set; }
        public Klant Klant { get; set; }
        public string BestellingNummer { get; set; }
        public ICollection<BestelRegel> BestelRegels { get; set; } = new List<BestelRegel>();
        public bool Goedgekeurd { get; set; }
        public bool Afgekeurd { get; set; }
        public bool KlaarGemeld { get; set; }
        public Adres AfleverAdres { get; set; }
        public DateTime BestelDatum { get; set; }
        public decimal Subtotaal { get; set; }
        public decimal SubtotaalMetVerzendKosten { get; set; }
        public decimal SubtotaalInclusiefBtw { get; set; }
        public decimal SubtotaalInclusiefBtwMetVerzendKosten { get; set; }
        public bool FactuurGeprint { get; set; }
        public bool AdresLabelGeprint { get; set; }
        public decimal OpenstaandBedrag { get; set; }
        public bool IsKlantWanbetaler { get; set; }

        public void ControleerOfBestellingAutomatischGoedgekeurdKanWorden()
        {
            decimal sumOpenstaandGoedgekeurdebestellingen = Klant.Bestellingen
                .Where(b => !b.Afgekeurd && !b.KlaarGemeld)
                .Sum(b => b.OpenstaandBedrag);

            if (sumOpenstaandGoedgekeurdebestellingen <= AutomatischGoedgekeurdMaximalePrijs
                && OpenstaandBedrag <= AutomatischGoedgekeurdMaximalePrijs)
            {
                KeurGoed();
            }
        }

        public void KeurGoed()
        {
            Goedgekeurd = true;
        }

        public void KeurAf()
        {
            Afgekeurd = true;
        }

        public virtual void ControleerOfKlantWanbetalerIs()
        {
            if (IsKlantWanbetaler || Goedgekeurd || Afgekeurd || BestelDatum.AddDays(DagenOmTeBetalen) >= DateTime.Now)
            {
                return;
            }

            IsKlantWanbetaler = true;
            OnBestellingKlantIsWanbetalerGeworden();
        }

        public void MeldKlaar()
        {
            if (!FactuurGeprint)
            {
                throw new BestellingKanNietKlaargemeldWordenException(this, FactuurIsNietUigeprintMessage);
            }

            if (!AdresLabelGeprint)
            {
                throw new BestellingKanNietKlaargemeldWordenException(this, AdresLabelIsNietUigeprintMessage);
            }

            if (!BestelRegels.All(e => e.Ingepakt))
            {
                throw new BestellingKanNietKlaargemeldWordenException(this, BestelRegelsZijnNogNietIngepaktMessage);
            }

            KlaarGemeld = true;
        }

        public void PakIn(long bestelRegelId)
        {
            BestelRegel bestelRegel = BestelRegels.Single(b => b.Id == bestelRegelId);
            bestelRegel.PakIn();

            if (KanKlaarGemeldWorden())
            {
                OnBestellingKanKlaargemeldWorden();
            }
        }

        public void VoegBestelnummerToe()
        {
            BestellingNummer = $"{Id + BestelNummerStartNumber}";
        }

        public void PrintFactuur()
        {
            FactuurGeprint = true;

            if (KanKlaarGemeldWorden())
            {
                OnBestellingKanKlaargemeldWorden();
            }
        }

        public void PrintAdresLabel()
        {
            AdresLabelGeprint = true;

            if (KanKlaarGemeldWorden())
            {
                OnBestellingKanKlaargemeldWorden();
            }
        }

        public void BoekBedragAf(decimal bedrag)
        {
            var newOpenstaand = OpenstaandBedrag - bedrag;
            if (newOpenstaand < 0)
            {
                throw new BedragKanNietWordenAfgeboektWordenException(this, OpenstaandBedragNietNegatief);
            }

            OpenstaandBedrag = newOpenstaand;
        }

        private bool KanKlaarGemeldWorden()
        {
            return FactuurGeprint && AdresLabelGeprint && BestelRegels.All(e => e.Ingepakt);
        }

        protected virtual void OnBestellingKanKlaargemeldWorden()
        {
            BestellingKanKlaargemeldWorden?.Invoke(this, new BestellingKanKlaarGemeldWordenEventArgs());
        }

        protected virtual void OnBestellingKlantIsWanbetalerGeworden()
        {
            BestellingKlantIsWanbetalerGeworden?.Invoke(this, new BestellingKlantIsWanBetalerGewordenEventArgs());
        }
    }

    public delegate void BestellingKlantIsWanBetalerGewordenEventHandler(Bestelling bestelling,
        BestellingKlantIsWanBetalerGewordenEventArgs args);
    public delegate void BestellingKanKlaargemeldWordenEventHandler(Bestelling bestelling,
        BestellingKanKlaarGemeldWordenEventArgs args);
}
