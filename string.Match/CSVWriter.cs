using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace @string.Match
{
    public static class CSVWriter
    {
        public static void WriteFile<T>(List<T> data, string path, Encoding encoding = null)
        {
            var csv = new StringBuilder();
            var properties = typeof(T).GetProperties();
            var header = string.Join(";", properties.Select(p => p.Name));
            csv.AppendLine(header);
            foreach (var item in data)
            {
                var line = string.Join(";", properties.Select(p => p.GetValue(item)?.ToString() ?? ""));
                csv.AppendLine(line);
            }
            File.WriteAllText(path, csv.ToString(), encoding ?? Encoding.UTF8);
        }
    }

}