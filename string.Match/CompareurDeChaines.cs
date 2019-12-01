﻿using System;
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

        public CompareurDeChaines()
        {
            _a = new HashSet<string>();
            _b = new HashSet<string>();
            _c = new HashSet<char> { '_', '-', ' ' };
        }

        public CompareurDeChaines(IEnumerable<string> a, IEnumerable<string> b)
        {
            _a = new HashSet<string>(a);
            _b = new HashSet<string>(b);
            _c = new HashSet<char> { '_', '-', ' ' };
        }

        public bool Match(string a, string b)
        {
            return Egualite(a, b) ||
                   EgualiteIgnoreCase(a, b) ||
                   EgualiteIgnoreCulture(a, b) ||
                   EgualiteIgnoreCultureFort(a, b) ||
                   EgualiteRemovingSetC(a, b);
        }

        private bool Egualite(string a, string b)
        {
            return a.Equals(b);
        }

        private bool EgualiteIgnoreCase(string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase)
                   && NotEqualsMatchInSets(a, b);
        }

        private bool EgualiteIgnoreCulture(string a, string b)
        {
            return a.Equals(b, StringComparison.InvariantCultureIgnoreCase)
                   && NotEqualsIgnoreCaseMatchInSets(a, b);
        }

        private bool EgualiteIgnoreCultureFort(string a, string b)
        {
            return string.Compare(a, b, CultureInfo.CurrentCulture,
                   CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
                   && NotEqualsIngoreCultureInSets(a, b);
        }

        private bool EgualiteRemovingSetC(string a, string b)
        {
            var ap = RemoveSetC(a);
            var bp = RemoveSetC(b);

            return ap.Equals(bp)
                   && NotEqualsIgnoreCultureFortMatchInSets(a, b);
        }

        private bool NotEqualsMatchInSets(string a, string b)
        {
            var match = false;

            foreach (var c in _a.Union(_b))
            {
                if (!string.ReferenceEquals(a, c)) 
                {
                    match = a.Equals(c);

                    if (match)
                    {
                        break;
                    }
                }
                if (!string.ReferenceEquals(b, c))
                {
                    match = b.Equals(c);

                    if (match)
                    {
                        break;
                    }
                }
            }

            return !match;
        }

        private bool NotEqualsIgnoreCaseMatchInSets(string a, string b)
        {
            var match = false;

            foreach (var c in _a.Union(_b))
            {
                if (!(string.ReferenceEquals(a, c) || string.ReferenceEquals(b, c)))
                {
                    match = EgualiteIgnoreCase(a, c);
                    match = EgualiteIgnoreCase(b, c);

                    if (match)
                    {
                        break;
                    }
                }
            }

            return !match;
        }

        private bool NotEqualsIngoreCultureInSets(string a, string b)
        {
            var match = false;

            foreach (var c in _a.Union(_b))
            {
                if (!(string.ReferenceEquals(a, c) || string.ReferenceEquals(b, c)))
                {
                    match = EgualiteIgnoreCulture(a, c);
                    match = EgualiteIgnoreCulture(b, c);

                    if (match)
                    {
                        break;
                    }
                }
            }

            return !match;
        }

        private bool NotEqualsIgnoreCultureFortMatchInSets(string a, string b)
        {
            var match = false;

            foreach (var c in _a.Union(_b))
            {
                if (!(string.ReferenceEquals(a, c) || string.ReferenceEquals(b, c)))
                {
                    match = EgualiteIgnoreCultureFort(a, c);
                    match = EgualiteIgnoreCultureFort(b, c);

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
