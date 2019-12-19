using @string.Match;
using System;
using System.Linq;

namespace ConsoleApp1
{
    public class Registre
    {
        public string Etudiant { get; set; }
        public string Jour { get; set; }
        public string Montant { get; set; }

        public override string ToString()
        {
            return $"{Etudiant};{Jour};{Montant}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var registre = CSVOpener.ReadFile<Registre>(args[0]);
            
            registre.GroupBy(r => r.Etudiant, new CompareurDeChaines(registre.Select(r => r.Etudiant)))
                    .Select(r => new 
                    {
                        Etudiant = r.Key,
                        Nombre = r.Count(),
                        Depense = r.Sum(c => double.Parse(c.Montant)),
                    })
                    .ForEach(r =>
                    {
                        Console.WriteLine("---");
                        Console.WriteLine(r.Etudiant);
                        Console.WriteLine($"Nombre  : {r.Nombre}");
                        Console.WriteLine($"Depense : {r.Depense}");
                    });
        }
    }
}
