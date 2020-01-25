using FrontendService.Events;
using FrontendService.Listeners;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Listeners
{
    [TestClass]
    public class MagazijnEventListenersTest
    {
        [TestMethod]
        [DataRow(28, 45)]
        [DataRow(29482, 76)]
        public void HandleVoorraadVerhoogd_CallsGetByArtikelNummerOnRepository(int artikelnummer, int voorraad)
        {
            // Arrange
            var klantRepositoryMock = new Mock<IArtikelRepository>();
            var listener = new MagazijnEventListeners(klantRepositoryMock.Object);
            var @event = new VoorraadVerhoogdEvent
            {
                Artikelnummer = artikelnummer,
                NieuweVoorraad = voorraad
            };

            Artikel artikel = new Artikel { Artikelnummer = artikelnummer, Voorraad = voorraad };
            klantRepositoryMock.Setup(e => e.GetByArtikelnummer(artikelnummer))
                .Returns(artikel)
                .Verifiable();

            // Act
            listener.HandleVoorraadVerhoogd(@event);

            // Assert
            klantRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(28, 45)]
        [DataRow(29482, 76)]
        public void HandleVoorraadVerhoogd_CallsUpdateVoorraadWithArtikel(int artikelnummer, int voorraad)
        {
            // Arrange
            var klantRepositoryMock = new Mock<IArtikelRepository>();
            var listener = new MagazijnEventListeners(klantRepositoryMock.Object);
            var @event = new VoorraadVerhoogdEvent
            {
                Artikelnummer = artikelnummer,
                NieuweVoorraad = voorraad
            };

            Artikel artikel = new Artikel { Artikelnummer = artikelnummer, Voorraad = voorraad };
            klantRepositoryMock.Setup(e => e.GetByArtikelnummer(artikelnummer))
                .Returns(artikel);

            // Act
            listener.HandleVoorraadVerhoogd(@event);

            // Assert
            klantRepositoryMock.Verify(e =>
                e.Update(It.Is<Artikel>(a => a.Artikelnummer == artikelnummer && a.Voorraad == voorraad)));
        }

        [TestMethod]
        [DataRow(28, 45)]
        [DataRow(29482, 76)]
        public void HandleVoorraadVerlaagd_CallsUpdateVoorraadWithArtikel(int artikelnummer, int voorraad)
        {
            // Arrange
            var klantRepositoryMock = new Mock<IArtikelRepository>();
            var listener = new MagazijnEventListeners(klantRepositoryMock.Object);
            var @event = new VoorraadVerlaagdEvent
            {
                Artikelnummer = artikelnummer,
                NieuweVoorraad = voorraad
            };

            Artikel artikel = new Artikel { Artikelnummer = artikelnummer, Voorraad = voorraad };
            klantRepositoryMock.Setup(e => e.GetByArtikelnummer(artikelnummer))
                .Returns(artikel);

            // Act
            listener.HandleVoorraadVerlaagd(@event);

            // Assert
            klantRepositoryMock.Verify(e =>
                e.Update(It.Is<Artikel>(a => a.Artikelnummer == artikelnummer && a.Voorraad == voorraad)));
        }

        [TestMethod]
        [DataRow(28)]
        [DataRow(29482)]
        public void HandleVoorraadVerlaagd_CallsGetByArtikelNummerOnRepository(int artikelnummer)
        {
            // Arrange
            var klantRepositoryMock = new Mock<IArtikelRepository>();
            var listener = new MagazijnEventListeners(klantRepositoryMock.Object);
            var @event = new VoorraadVerlaagdEvent
            {
                Artikelnummer = artikelnummer
            };

            Artikel artikel = new Artikel { Artikelnummer = artikelnummer };
            klantRepositoryMock.Setup(e => e.GetByArtikelnummer(artikelnummer))
                .Returns(artikel)
                .Verifiable();

            // Act
            listener.HandleVoorraadVerlaagd(@event);

            // Assert
            klantRepositoryMock.Verify();
        }
    }
}
