using BackOfficeFrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Models
{
    [TestClass]
    public class BestellingTest
    {
        [TestMethod]
        public void VoorraadBeschikbaar_IsTrueWithNoBestelRegels()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();

            // Act
            bool result = bestelling.VoorraadBeschikbaar;

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void VoorraadBeschikbaar_IsTrueWithAllBestelRegelsOpVoorraad()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelRegels =
                {
                    new BestelRegel
                    {
                        Aantal = 5,
                        Voorraad = new VoorraadMagazijn { Voorraad = 5 }
                    },
                    new BestelRegel
                    {
                        Aantal = 8,
                        Voorraad = new VoorraadMagazijn { Voorraad = 13 }
                    }
                }
            };

            // Act
            bool result = bestelling.VoorraadBeschikbaar;

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void VoorraadBeschikbaar_IsFalseIfNotAllBestelRegelsOpVoorraad()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelRegels =
                {
                    new BestelRegel
                    {
                        Aantal = 5,
                        Voorraad = new VoorraadMagazijn { Voorraad = 5 }
                    },
                    new BestelRegel
                    {
                        Aantal = 8,
                        Voorraad = new VoorraadMagazijn { Voorraad = 2 }
                    }
                }
            };

            // Act
            bool result = bestelling.VoorraadBeschikbaar;

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void VoorraadBeschikbaar_IsFalseIfNoBestelRegelsOpVoorraad()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelRegels =
                {
                    new BestelRegel
                    {
                        Aantal = 5,
                        Voorraad = new VoorraadMagazijn { Voorraad = 1 }
                    },
                    new BestelRegel
                    {
                        Aantal = 8,
                        Voorraad = new VoorraadMagazijn { Voorraad = 2 }
                    }
                }
            };

            // Act
            bool result = bestelling.VoorraadBeschikbaar;

            // Assert
            Assert.AreEqual(false, result);
        }
    }

    [TestClass]
    public class BestellingEqualityComparerTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        public void Equals_BestellingWithSameIdsAreEqual(long id)
        {
            // Arrange
            Bestelling bestellingA = new Bestelling {Id = id};
            Bestelling bestellingB = new Bestelling {Id = id};

            BestellingEqualityComparer comparer = new BestellingEqualityComparer();

            // Act
            bool result = comparer.Equals(bestellingA, bestellingB);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [DataRow(1, 3)]
        [DataRow(5, 20)]
        public void Equals_BestellingWithDifferentIdsAreNotEqual(long idA, long idB)
        {
            // Arrange
            Bestelling bestellingA = new Bestelling {Id = idA};
            Bestelling bestellingB = new Bestelling {Id = idB};

            BestellingEqualityComparer comparer = new BestellingEqualityComparer();

            // Act
            bool result = comparer.Equals(bestellingA, bestellingB);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_ComparingWithNullReturnsFalse()
        {
            // Arrange

            BestellingEqualityComparer comparer = new BestellingEqualityComparer();

            // Act
            bool result = comparer.Equals(new Bestelling(), null);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_ComparingNullWithBestellingReturnsFalse()
        {
            // Arrange
            BestellingEqualityComparer comparer = new BestellingEqualityComparer();

            // Act
            bool result = comparer.Equals(null, new Bestelling());

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(29)]
        public void GetHashCode_BestellingenWithSameIdsAreEqual(long id)
        {
            // Arrange
            Bestelling bestellingA = new Bestelling {Id = id};
            Bestelling bestellingB = new Bestelling {Id = id};

            BestellingEqualityComparer comparer = new BestellingEqualityComparer();

            // Act
            bool result = comparer.GetHashCode(bestellingA) == comparer.GetHashCode(bestellingB);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [DataRow(1, 23)]
        [DataRow(29, 10)]
        public void GetHashCode_BestellingenWithDifferentIdsAreNotEqual(long idA, long idB)
        {
            // Arrange
            Bestelling bestellingA = new Bestelling {Id = idA};
            Bestelling bestellingB = new Bestelling {Id = idB};

            BestellingEqualityComparer comparer = new BestellingEqualityComparer();

            // Act
            bool result = comparer.GetHashCode(bestellingA) == comparer.GetHashCode(bestellingB);

            // Assert
            Assert.AreEqual(false, result);
        }
    }
}
