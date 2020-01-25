using System;
using System.Collections.Generic;
using FrontendService.Constants;
using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace FrontendService.Spec.Bestelling.Steps
{
    [Binding]
    [Scope(Scenario = "Er komt een bestelling binnen met een subtotaal")]
    public class ErKomtEenBestellingBinnenMetEenSubtotaal
    {
        private Models.Bestelling _bestelling;
        private decimal _totaalInclusiefBtw;
        private decimal _totaalExclusiefBtw;

        [Given(@"Een bestelling met een subtotaal van (.*)")]
        public void GivenEenBestellingMetEenSubtotaalVanEnEenSubtotaalInclusiefBtwVan(Decimal p0)
        {
            _bestelling = new Models.Bestelling
            {
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Aantal = 1, StukPrijs = p0 / SystemVariables.BtwMultiplier }
                }
            };
        }

        [When(@"Ik het subtotaal met verzendkosten inclusief en exclusief bereken")]
        public void WhenIkHetSubtotaalMetVerzendkostenInclusiefEnExclusiefBereken()
        {
            _totaalExclusiefBtw = _bestelling.SubtotaalMetVerzendKosten;
            _totaalInclusiefBtw = _bestelling.SubtotaalInclusiefBtwMetVerzendKosten;
        }

        [Then(@"Zou het subtotaal exclusief btw met verzendkosten (.*) moeten zijn")]
        public void ThenZouHetSubtotaalExclusiefBtwMetVerzendkostenMoetenZijn(Decimal p0)
        {
            Assert.AreEqual(p0, _totaalExclusiefBtw);
        }

        [Then(@"Zou het subtotaal met verzendkosten (.*) moeten zijn")]
        public void ThenZouHetSubtotaalMetVerzendkostenMoetenZijn(Decimal p0)
        {
            Assert.AreEqual(p0, _totaalInclusiefBtw);
        }
    }
}
