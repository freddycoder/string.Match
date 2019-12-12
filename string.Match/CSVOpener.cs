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
        public static List<T> ReadFile<T>(string path, char separator = ';', Encoding encoding = null) where T : class
        {
            var list = new List<T>();

            var lines = File.ReadAllText(path, encoding ?? Encoding.Default)
                            .ParseToLines();

            // La clé représente la colonne et la valeur la propriété obtenue par reflexion
            var properties = ParseHeader<T>(lines[0], separator);

            lines.ForEach((line, l) =>
            {
                if (l > 0)
                {
                    var @object = Activator.CreateInstance<T>();

                    line.Split(separator).ForEach((value, i) =>
                    {
                        if (properties.ContainsKey(i))
                        {
                            properties[i].SetValue(@object, value);
                        }
                    });

                    list.Add(@object);
                }
            });

            return list;
        }

        private static Dictionary<int, PropertyInfo> ParseHeader<T>(string headerLine, char separator)
        {
            var indexToProperty = new Dictionary<int, PropertyInfo>();
            var titles = headerLine.Split(separator);
            var properties = typeof(T).GetProperties();
            var compareur = new CompareurDeChaines(titles, properties.Select(p => p.Name));

            titles.ForEach((title, i) =>
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var propriete = properties.FirstOrDefault(p => compareur.Match(title, p.Name));

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
