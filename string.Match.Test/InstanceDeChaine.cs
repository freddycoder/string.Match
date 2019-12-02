using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace @string.Match.Test
{
    [TestClass]
    public class InstanceDeChaine
    {
        public class SpecialCase
        {
            public string Écriture { get; set; }
            public string Ecriture { get; set; }
        }

        [TestMethod]
        public void HardCodedVsStringObtenuParReflexion()
        {
            var setA = typeof(SpecialCase).GetProperties().Select(p => p.Name).ToList();
            var setB = new List<string> { "Écriture", "Ecriture" };

            var compareur = new CompareurDeChaines(setA, setB);

            Assert.AreEqual(2, setA.Count);

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

        public class SpecialCase2
        {
            public string Frédéric { get; set; }
            public string Frederic { get; set; }
        }

        [TestMethod]
        public void HardCodedVsStringObtenuParReflexion2()
        {
            var setA = typeof(SpecialCase2).GetProperties().Select(p => p.Name).ToList();
            var setB = new List<string> { "frédéric", "frederic" };

            var compareur = new CompareurDeChaines(setA, setB);

            Assert.AreEqual(2, setA.Count);
            Assert.AreEqual("Frédéric", setA[0]);

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
                        Assert.IsFalse(compareur.Match(setA[i], setB[j]), $"{setA[i]} ne peut pas aller avec {setB[j]}");
                    }
                }
            }
        }
    }
}
