using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace @string.Match
{
    public class CSVOpener
    {
        public static List<T> ReadFile<T>(string path) where T : class
        {
            var list = new List<T>();

            var text = File.ReadAllText(path, Encoding.Default);

            var lines = text.ParseToLines();

            // La clé représente la colonne et la valeur la propriété obtenue par reflexion
            var properties = ParseHeader<T>(lines[0]);

            var firstLine = true;

            lines.ForEach(line =>
            {
                if (!firstLine)
                {
                    var transaction = Activator.CreateInstance<T>();

                    line.Split(';').ForEach((e, i) =>
                    {
                        if (properties.ContainsKey(i))
                        {
                            properties[i].SetValue(transaction, e);
                        }
                    });

                    list.Add(transaction);
                }
                else
                {
                    firstLine = false;
                }
            });

            return list;
        }

        private static Dictionary<int, PropertyInfo> ParseHeader<T>(string headerLine)
        {
            var indexToProperty = new Dictionary<int, PropertyInfo>();
            var titles = headerLine.Split(';');
            var properties = typeof(T).GetProperties();
            var compareur = new CompareurDeChaines(titles, properties.Select(p => p.Name));

            titles.ForEach((title, i) =>
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var propriete = properties.First(p => compareur.Match(title, p.Name));

                    if (propriete != null)
                    {
                        indexToProperty.Add(i, propriete);
                    }
                }
            });

            return indexToProperty;
        }
    }
}
