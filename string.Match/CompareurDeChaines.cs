using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace @string.Match
{
    public static class CompareurDeChaineExtension
    {
        private readonly static CompareurDeChaines _compareur = new CompareurDeChaines();

        public static bool Match(this string a, string b)
        {
            return _compareur.Match(a, b);
        }
    }

    public class CompareurDeChaines
    {
        private readonly HashSet<string> _a;
        private readonly HashSet<string> _b;
        private readonly HashSet<char> _c;
        private readonly List<Func<string, string, bool>> _predicats;

        /// <summary>
        /// Constrcteur par défaut. Les ensemble A et B seront.
        /// </summary>
        public CompareurDeChaines()
        {
            _a = new HashSet<string>();
            _b = new HashSet<string>();
            _c = new HashSet<char> { '_', '-', ' ' };
            _predicats = new List<Func<string, string, bool>>
            {
                Egualite,
                EgualiteIgnoreCase,
                EgualiteIgnoreCulture,
                EgualiteIgnoreCultureFort,
                EgualiteRemovingSetC
            };
        }

        /// <summary>
        /// Constructeur par initialisation a comparant avec deux essembles de valeurs
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public CompareurDeChaines(IEnumerable<string> a, IEnumerable<string> b)
        {
            _a = new HashSet<string>(a);
            _b = new HashSet<string>(b);
            _c = new HashSet<char> { '_', '-', ' ' };
            _predicats = new List<Func<string, string, bool>>
            {
                Egualite,
                EgualiteIgnoreCase,
                EgualiteIgnoreCulture,
                EgualiteIgnoreCultureFort,
                EgualiteRemovingSetC
            };
        }

        /// <summary>
        /// Constructeur d'initialisation permetant de spécifier l'ensemble de caractère
        /// à ignorer
        /// </summary>
        /// <param name="a">L'ensemble de valeur A</param>
        /// <param name="b">L'ensemble de valeur B</param>
        /// <param name="c">L'ensemble des caractère à ignorer</param>
        public CompareurDeChaines(IEnumerable<string> a, IEnumerable<string> b, IEnumerable<char> c)
        {
            _a = new HashSet<string>(a);
            _b = new HashSet<string>(b);
            _c = new HashSet<char>(c);
            _predicats = new List<Func<string, string, bool>>
            {
                Egualite,
                EgualiteIgnoreCase,
                EgualiteIgnoreCulture,
                EgualiteIgnoreCultureFort,
                EgualiteRemovingSetC
            };
        }

        /// <summary>
        /// Constructeur d'initialisation, d'assigner chaque données membres
        /// de la classe
        /// </summary>
        /// <param name="a">L'ensemble de valeur A</param>
        /// <param name="b">L'ensemble de valeur B</param>
        /// <param name="c">Les caractères à ignorer</param>
        /// <param name="predicats">Une liste de predicats</param>
        public CompareurDeChaines(IEnumerable<string> a, 
                                  IEnumerable<string> b, 
                                  IEnumerable<char> c, 
                                  List<Func<string, string, bool>> predicats)
        {
            _a = new HashSet<string>(a);
            _b = new HashSet<string>(b);
            _c = new HashSet<char>(c);
            _predicats = predicats;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Match(string a, string b)
        {
            bool match = false;

            for (int i = 0; i < _predicats.Count && !match; i++)
            {
                match = _predicats[i].Invoke(a, b);

                if (match && i > 0)
                {
                    match = NoMatchInSets(a, b, _predicats[i - 1]);
                }
            }

            return match;
        }

        private bool Egualite(string a, string b)
        {
            return a.Equals(b);
        }

        private bool EgualiteIgnoreCase(string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }

        private bool EgualiteIgnoreCulture(string a, string b)
        {
            return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool EgualiteIgnoreCultureFort(string a, string b)
        {
            return string.Compare(a, b, CultureInfo.CurrentCulture,
                   CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
        }

        private bool EgualiteRemovingSetC(string a, string b)
        {
            var ap = RemoveSetC(a);
            var bp = RemoveSetC(b);

            return ap.Equals(bp);
        }

        private bool NoMatchInSets(string a, string b, Func<string, string, bool> func)
        {
            var match = false;

            foreach (var c in _a.Union(_b))
            {
                if (!ReferenceEquals(a, c) && !ReferenceEquals(b, c)) 
                {
                    match = func.Invoke(a, c) || func.Invoke(b, c);

                    if (match)
                    {
                        break;
                    }
                }
            }

            return !match;
        }

        private string RemoveSetC(string a)
        {
            a = a.ToLower(CultureInfo.InvariantCulture);

            foreach (var c in _c)
            {
                a = a.Replace(c.ToString().ToLower(), "");
            }

            return a;
        }
    }
}
