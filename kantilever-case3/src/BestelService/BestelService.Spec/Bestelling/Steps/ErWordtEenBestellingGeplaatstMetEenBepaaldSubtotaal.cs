using System;
using BestelService.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BestelService.Spec.Bestelling.Steps
{
    [Binding]
    [Scope(Scenario = "Er wordt een bestelling geplaatst met een bepaald subtotaal")]
    public class ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal
    {
        private Core.Models.Bestelling _bestelling;

        [Given(@"Een bestelling van (.*)")]
        public void GivenEenBestellingVan(Decimal subtotaalInclusiefBtw)
        {
            _bestelling = new Core.Models.Bestelling { OpenstaandBedrag = subtotaalInclusiefBtw };
            _bestelling.Klant = new Klant();
            _bestelling.Klant.Bestellingen.Add(_bestelling);
        }

        [When(@"Deze geplaatst wordt")]
        public void WhenDezeGeplaatstWordt()
        {
            _bestelling.ControleerOfBestellingAutomatischGoedgekeurdKanWorden();
        }

        [Then(@"Moet deze wel automatisch goedgekeurd worden")]
        public void ThenMoetDezeWelAutomatischGoedgekeurdWorden()
        {
            Assert.AreEqual(true, _bestelling.Goedgekeurd);
        }

        [Then(@"Moet deze niet automatisch goedgekeurd worden")]
        public void ThenMoetDezeNietAutomatischGoedgekeurdWorden()
        {
            Assert.AreEqual(false, _bestelling.Goedgekeurd);
        }
    }
}
