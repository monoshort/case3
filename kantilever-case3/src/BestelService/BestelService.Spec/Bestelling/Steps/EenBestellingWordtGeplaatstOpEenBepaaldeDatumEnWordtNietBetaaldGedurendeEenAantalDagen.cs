using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BestelService.Spec.Bestelling.Steps
{
    [Binding]
    [Scope(Scenario = "Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedurende een aantal dagen")]
    public class EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen
    {
        private Core.Models.Bestelling _bestelling;

        [Given(@"Een niet automatisch goedgekeurde xrandbestelling die (.*) dagen geleden is geplaatst")]
        public void GivenEenNietAutomatischGoedgekeurdeXrandbestellingDie(int aantalDagenGeleden)
        {
            _bestelling = new Core.Models.Bestelling
            {
                BestelDatum = DateTime.Now.AddDays(-aantalDagenGeleden)
            };
        }

        [When(@"Er opgevraagd wordt of dit een bestelling met wanbetaler betreft")]
        public void WhenErOpgevraagdWordtOfDitEenBestellingMetWanbetalerBetreft()
        {
            _bestelling.ControleerOfKlantWanbetalerIs();
        }

        [Then(@"Moet de bestelling wel gemarkeerd worden als wanbetaler")]
        public void ThenMoetDeBestellingWelGemarkeerdWordenAlsWanbetaler()
        {
            Assert.AreEqual(true, _bestelling.IsKlantWanbetaler);
        }

        [Then(@"Moet de bestelling niet gemarkeerd worden als wanbetaler")]
        public void ThenMoetDeBestellingNietGemarkeerdWordenAlsWanbetaler()
        {
            Assert.AreEqual(false, _bestelling.IsKlantWanbetaler);
        }
    }
}
