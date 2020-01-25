using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.EventListeners
{
    [TestClass]
    public class VoorraadEventListenersTest
    {
        [TestMethod]
        [DataRow(10)]
        [DataRow(592)]
        public void HandleVoorraadBesteld_CallsGetByArtikelNummerOnRepository(long artikelNummer)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            VoorraadEventListeners listeners = new VoorraadEventListeners(voorraadRepositoryMock.Object);

            VoorraadBesteldEvent evt = new VoorraadBesteldEvent
            {
                Artikelnummer = artikelNummer,
                BesteldeVoorraad = 20
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(new VoorraadMagazijn())
                .Verifiable();

            // Act
            listeners.HandleVoorraadBesteld(evt);

            // Assert
            voorraadRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(592)]
        public void HandleVoorraadBesteld_CallsUpdateOnRepositoryWithVoorraadBeteldTrue(long artikelNummer)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            VoorraadEventListeners listeners = new VoorraadEventListeners(voorraadRepositoryMock.Object);

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                VoorraadBesteld = true
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(voorraadMagazijn);

            VoorraadBesteldEvent evt = new VoorraadBesteldEvent
            {
                Artikelnummer = artikelNummer,
                BesteldeVoorraad = 20
            };

            // Act
            listeners.HandleVoorraadBesteld(evt);

            // Assert
            voorraadRepositoryMock.Verify(e =>
                e.Update(It.Is<VoorraadMagazijn>(v => v.VoorraadBesteld)));
        }

        [TestMethod]
        [DataRow(10, 20)]
        [DataRow(592, 300)]
        public void HandleVoorraadVerlaagd_CallsGetByArtikelNummerOnRepository(long artikelNummer, int newAmount)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            VoorraadEventListeners listeners = new VoorraadEventListeners(voorraadRepositoryMock.Object);

            VoorraadVerlaagdEvent evt = new VoorraadVerlaagdEvent
            {
                Artikelnummer = artikelNummer,
                NieuweVoorraad = newAmount
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(new VoorraadMagazijn())
                .Verifiable();

            // Act
            listeners.HandleVoorraadVerlaagd(evt);

            // Assert
            voorraadRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(10, 11)]
        [DataRow(592, 29)]
        public void HandleVoorraadVerlaagd_CallsUpdateOnRepositoryWithNewVoorraad(long artikelNummer, int newAmount)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            VoorraadEventListeners listeners = new VoorraadEventListeners(voorraadRepositoryMock.Object);

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                VoorraadBesteld = true
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(voorraadMagazijn);

            VoorraadVerlaagdEvent evt = new VoorraadVerlaagdEvent
            {
                Artikelnummer = artikelNummer,
                NieuweVoorraad = newAmount
            };

            // Act
            listeners.HandleVoorraadVerlaagd(evt);

            // Assert
            voorraadRepositoryMock.Verify(e =>
                e.Update(It.Is<VoorraadMagazijn>(v => v.Voorraad == newAmount)));
        }

        [TestMethod]
        [DataRow(10, 20)]
        [DataRow(592, 300)]
        public void HandleVoorraadVerhoogd_CallsGetByArtikelNummerOnRepository(long artikelNummer, int newAmount)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            VoorraadEventListeners listeners = new VoorraadEventListeners(voorraadRepositoryMock.Object);

            VoorraadVerhoogdEvent evt = new VoorraadVerhoogdEvent
            {
                Artikelnummer = artikelNummer,
                NieuweVoorraad = newAmount
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(new VoorraadMagazijn())
                .Verifiable();

            // Act
            listeners.HandleVoorraadVerhoogd(evt);

            // Assert
            voorraadRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(10, 11)]
        [DataRow(592, 29)]
        public void HandleVoorraadVerhoogd_CallsUpdateOnRepositoryWithVoorraadAndVoorraadBeteldFalse(long artikelNummer, int newAmount)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            VoorraadEventListeners listeners = new VoorraadEventListeners(voorraadRepositoryMock.Object);

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                VoorraadBesteld = true
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(voorraadMagazijn);

            VoorraadVerhoogdEvent evt = new VoorraadVerhoogdEvent
            {
                Artikelnummer = artikelNummer,
                NieuweVoorraad = newAmount
            };

            // Act
            listeners.HandleVoorraadVerhoogd(evt);

            // Assert
            voorraadRepositoryMock.Verify(e =>
                e.Update(It.Is<VoorraadMagazijn>(v => v.Voorraad == newAmount && !v.VoorraadBesteld)));
        }
    }
}
