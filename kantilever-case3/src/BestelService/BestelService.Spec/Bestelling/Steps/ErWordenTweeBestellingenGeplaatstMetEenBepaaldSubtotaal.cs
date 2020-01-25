using System;
using BestelService.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BestelService.Spec.Bestelling.Steps
{
    [Binding]
    [Scope(Scenario = "Er worden twee bestellingen geplaatst met een bepaald subtotaal")]
    public class ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal
    {
        private Core.Models.Bestelling _bestelling1;
        private Core.Models.Bestelling _bestelling2;
        private Klant _klant;

        [Given(@"Een bestelling van een klant met een subtotaal inclusief btw van (.*)")]
        public void GivenEenBestellingVanEenKlantMetEenSubtotaalInclusiefBtwVan(Decimal p0)
        {
            _klant = new Klant();
            _bestelling1 = new Core.Models.Bestelling
            {
                Klant = _klant,
                OpenstaandBedrag = p0
            };
        }

        [Given(@"Een bestelling van dezelfde klant met een subtotaal inclusief btw van (.*)")]
        public void GivenEenBestellingVanDezelfdeKlantMetEenSubtotaalInclusiefBtwVan(Decimal p0)
        {
            _bestelling2 = new Core.Models.Bestelling
            {
                Klant = _klant,
                OpenstaandBedrag = p0
            };
        }

        [When(@"Deze achter elkaar geplaatst worden")]
        public void WhenDezeAchterElkaarGeplaatstWorden()
        {
            _klant.Bestellingen.Add(_bestelling1);
            _bestelling1.ControleerOfBestellingAutomatischGoedgekeurdKanWorden();

            _klant.Bestellingen.Add(_bestelling2);
            _bestelling2.ControleerOfBestellingAutomatischGoedgekeurdKanWorden();
        }

        [Then(@"Moet bestelling een wel automatisch goedgekeurd worden en bestelling twee wel automatisch goedgekeurd worden")]
        public void ThenMoetBestellingEenWelAutomatischGoedgekeurdWordenEnBestellingTweeWelAutomatischGoedgekeurdWorden()
        {
            Assert.AreEqual(true, _bestelling1.Goedgekeurd);
            Assert.AreEqual(true, _bestelling2.Goedgekeurd);
        }

        [Then(@"Moet bestelling een niet automatisch goedgekeurd worden en bestelling twee niet automatisch goedgekeurd worden")]
        public void ThenMoetBestellingEenNietAutomatischGoedgekeurdWordenEnBestellingTweeNietAutomatischGoedgekeurdWorden()
        {
            Assert.AreEqual(false, _bestelling1.Goedgekeurd);
            Assert.AreEqual(false, _bestelling2.Goedgekeurd);
        }

        [Then(@"Moet bestelling een wel automatisch goedgekeurd worden en bestelling twee niet automatisch goedgekeurd worden")]
        public void ThenMoetBestellingEenWelAutomatischGoedgekeurdWordenEnBestellingTweeNietAutomatischGoedgekeurdWorden()
        {
            Assert.AreEqual(true, _bestelling1.Goedgekeurd);
            Assert.AreEqual(false, _bestelling2.Goedgekeurd);
        }
    }
}
