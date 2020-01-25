using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.EventListeners
{
    [TestClass]
    public class CatalogusEventListenersTest
    {
        [TestMethod]
        [DataRow(10, "test leverancier", "TL")]
        [DataRow(2, "test", "TT")]
        public void HandleArtikelAanCatalogusToegevoegd_CallsAddOnRepositoryWithValues(long artikelNummer, string leverancier, string leverancierCode)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            CatalogusEventListeners listener = new CatalogusEventListeners(voorraadRepositoryMock.Object);

            ArtikelAanCatalogusToegevoegdEvent evt = new ArtikelAanCatalogusToegevoegdEvent
            {
                Artikelnummer = artikelNummer,
                Leverancier = leverancier,
                Leveranciercode = leverancierCode
            };

            // Act
            listener.HandleArtikelAanCatalogusToegevoegd(evt);

            // Assert
            voorraadRepositoryMock.Verify(e =>
                e.Add(It.Is<VoorraadMagazijn>(v =>
                    v.Leverancier == leverancier &&
                    v.Leveranciercode == leverancierCode &&
                    v.ArtikelNummer == artikelNummer &&
                    v.Voorraad == 0 &&
                    !v.VoorraadBesteld)));
        }
    }
}
