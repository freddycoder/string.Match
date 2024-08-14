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

            string[] lines;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, encoding ?? Encoding.Default))
            {
                lines = sr.ReadToEnd().ParseToLines();
            }

            // La clé représente la colonne et la valeur la propriété obtenue par reflexion
            var properties = ParseHeader<T>(lines[0], separator);

            lines.ForEach((line, l) =>
            {
                if (line.EndsWith("\r"))
                {
                    line = line.Substring(0, line.Length - 1);
                }

                if (l > 0)
                {
                    var @object = Activator.CreateInstance<T>();

                    var cols = CsvSplit(line, separator);

                    cols.ForEach((value, i) =>
                    {
                        if (properties.ContainsKey(i))
                        {
                            try
                            {
                                if (properties[i].PropertyType == typeof(string))
                                {
                                    properties[i].SetValue(@object, value);
                                }
                                else if (properties[i].PropertyType == typeof(int))
                                {
                                    properties[i].SetValue(@object, int.Parse(value));
                                }
                                else if (properties[i].PropertyType == typeof(double))
                                {
                                    properties[i].SetValue(@object, double.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
                                }
                                else if (properties[i].PropertyType == typeof(DateTime))
                                {
                                    properties[i].SetValue(@object, DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                            catch (Exception e)
                            {
                                Console.Error.WriteLine($"Erreur lors de la lecture de la ligne {l} et de la colonne {i} : {e.Message}");
                                Console.Error.WriteLine($"La valeur {value} n'a pas pu être convertie en {properties[i].PropertyType}");
                                Console.Error.WriteLine(cols.Aggregate((a, b) => $"{a};{b}"));
                                Console.Error.WriteLine(line);
                            }
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

        public static string[] CsvSplit(string line, char separator)
        {
            var values = new List<string>();
            var sb = new StringBuilder();
            bool escape = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    escape = !escape;
                }
                else if (line[i] == separator && !escape)
                {
                    values.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(line[i]);
                }
            }

            values.Add(sb.ToString());

            return values.ToArray();
        }
    }
}
