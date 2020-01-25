using FrontendService.Events;
using FrontendService.Listeners;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Listeners
{
    [TestClass]
    public class CatalogusEventListenersTest
    {
        [TestMethod]
        [DataRow(28, "artikelNaam")]
        [DataRow(29482, "artikelNaam2")]
        public void HandleArtikelToegevoegd_CallsAdd(int artikelnummer, string naam)
        {
            // Arrange
            var artikel = new Artikel
            {
                Artikelnummer = artikelnummer,
                Naam = naam
            };
            var klantRepositoryMock = new Mock<IArtikelRepository>();
            klantRepositoryMock.Setup(r => r.Add(It.IsAny<Artikel>())).Verifiable();

            var listener = new CatalogusEventListeners(klantRepositoryMock.Object);
            var @event = new ArtikelAanCatalogusToegevoegdEvent
            {
                Artikelnummer = artikelnummer,
                Naam = naam
            };

            // Act
            listener.HandleArtikelToegevoegd(@event);

            // Assert
            klantRepositoryMock.Verify();
        }
    }
}
