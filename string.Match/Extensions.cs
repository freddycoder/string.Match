using System;
using System.Collections.Generic;
using System.Text;

namespace @string.Match
{
    /// <summary>
    /// Class contenant les method d'extension du projet
    /// </summary>
	public static class Extensions
    {
        /// <summary>
        /// Effectue un foreach
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// Effectue un foreach avec index
        /// </summary>
        public static void ForEach<T>(this IList<T> list, Action<T, int> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action.Invoke(list[i], i);
            };
        }

        /// <summary>
        /// Permet de lire une chaine de text en tableau de ligne en ignorant les sauts de lignes entre ""
        /// L'exemple suivant va retourner une seul ligne au lieu de deux comme le fait dans la methode static
        /// File.ReadLines
        /// exemple : a"\n"a
        /// </summary>
        /// <param name="text"></param>
        public static string[] ParseToLines(this string text)
        {
            var lines = new List<string>(text.Split('\n'));

            bool escapeEndl = false;

            var sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                if (!escapeEndl && text[i] == '\n')
                {
                    lines.Add(sb.ToString());

                    sb.Clear();
                }
                else if (text[i] == '"')
                {
                    escapeEndl = !escapeEndl;
                }
                else
                {
                    sb.Append(text[i]);
                }
            }

            return lines.ToArray();
        }
    }
}