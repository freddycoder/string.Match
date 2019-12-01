using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace @string.Match.Test
{
    [TestClass]
    public class ScenariosDeBase
    {
        [TestMethod]
        public void Scenario1()
        {
            var setA = new List<string> { "Scénario", "Horo_Date", "Service Domaine-Registres", "TransactionId", "DHMOCC", "DHMOCM" };
            var setB = new List<string> { "Scenario", "HoroDate", "ServiceDomaineRegistres", "Transaction-id", "DhmOcc", "DhmOcm" };

            var compareur = new CompareurDeChaines(setA, setB);

            for (int i = 0; i < setA.Count; i++)
            {
                Assert.IsTrue(compareur.Match(setA[i], setB[i]), $"{setA[i]} peut aller avec {setB[i]}");
            }
        }

        [TestMethod]
        public void BesionInitial()
        {
            var setA = new List<string> { "Scénario", "Description", "HORODATE", "TRANSACTIONID", "NOM", "Service Domaine-Registres", "XmlElementIn", "TRACKINGID", "CREATE_TIMESTAMP", "HIAL_MESSAGE_ID", "ORDER_NUMBER", "DISPENSE_NUMBER" };
            var setB = new List<string> { "Scenario", "Description", "HoroDate", "TransactionId", "Nom", "ServiceDomaineRegistres", "XmlElementIn", "TrackingId", "CreateTimeStamp", "HialMessageId", "OrderNumber", "DispenseNumber" };

            var compareur = new CompareurDeChaines(setA, setB);

            for (int i = 0; i < setA.Count; i++)
            {
                for (int j = 0; j < setB.Count; j++)
                {
                    if (i == j)
                    {
                        Assert.IsTrue(compareur.Match(setA[i], setB[i]), $"{setA[i]} peut aller avec {setB[i]}");
                    }
                    else
                    {
                        Assert.IsFalse(compareur.Match(setA[i], setB[j]), $"{setA[i]} ne peut pas aller avec {setB[i]}");
                    }
                }
            }
        }

        [TestMethod]
        public void Scenario1MethodExtension()
        {
            var setA = new List<string> { "Scénario", "Horo_Date", "Service Domaine-Registres", "TransactionId", "DHMOCC", "DHMOCM" };
            var setB = new List<string> { "Scenario", "HoroDate", "ServiceDomaineRegistres", "Transaction-id", "DhmOcc", "DhmOcm" };

            for (int i = 0; i < setA.Count; i++)
            {
                Assert.IsTrue(setA[i].Match(setB[i]), $"{setA[i]} peut aller avec {setB[i]}");
            }
        }

        [TestMethod]
        public void ScenarioUtilisationDesEmsembles1()
        {
            var setA = new List<string> { "Écriture", "Ecriture" };
            var setB = new List<string> { "Écriture", "Ecriture" };

            var compareur = new CompareurDeChaines(setA, setB);

            Assert.IsTrue(compareur.Match(setA[0], setB[0]), $"{setA[0]} peut aller avec {setB[0]}");
            Assert.IsTrue(compareur.Match(setA[1], setB[1]), $"{setA[1]} peut aller avec {setB[1]}");
        }

        [TestMethod]
        public void ScenarioUtilisationDesEmsembles2()
        {
            var setA = new List<string> { "Écriture", "Ecriture" };
            var setB = new List<string> { "Écriture", "Ecriture" };

            var compareur = new CompareurDeChaines(setA, setB);

            Assert.IsFalse(compareur.Match(setA[0], setB[1]), $"{setA[0]} ne peut pas aller avec {setB[1]}");
            Assert.IsFalse(compareur.Match(setA[1], setB[1]), $"{setA[1]} ne peut pas aller avec {setB[0]}");
        }

        [TestMethod]
        public void ScenarioUtilisationDesEmsembles3()
        {
            var setA = new List<string> { "Frédéric", "Frederic" };
            var setB = new List<string> { "frédéric", "frederic" };

            var compareur = new CompareurDeChaines(setA, setB);

            Assert.IsFalse(compareur.Match(setA[0], setB[1]), $"{setA[0]} ne peut pas aller avec {setB[1]}");
            Assert.IsFalse(compareur.Match(setA[1], setB[1]), $"{setA[1]} ne peut pas aller avec {setB[0]}");
        }

        [TestMethod]
        public void ScenarioUtilisationDesEmsembles4()
        {
            var setA = new List<string> { "Frédéric", "Frederic" };
            var setB = new List<string> { "frédéric", "frederic" };

            var compareur = new CompareurDeChaines(setA, setB);

            Assert.IsTrue(compareur.Match(setA[0], setB[0]), $"{setA[0]} peut aller avec {setB[0]}");
            Assert.IsTrue(compareur.Match(setA[1], setB[1]), $"{setA[1]} peut aller avec {setB[1]}");
        }
    }
}
