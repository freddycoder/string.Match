# string.Match
Permet de comparer des chaines de texte en prennant en compte des ensembles de valeurs

La DLL fournit une method d'extention et une classe. La méthode d'extension ne prend pas en compte les ensembles.

Les règles suivent l'ordre suivant : 
1. Parfaitement éguale
2. Éguale en ignorant la casse
3. Éguale en ingnorant la culture
4. Éguale en ingorant la culture fort
5. Égual en ingorant la culture fort et en enlevant les '_', '-' et les espaces.

En utilisant une instance de la classe CompareurDeChaines, pour qu'une chaine soit dit Identique,
il faut que pour l'ensemble des chaines, il en ait aucune qui valide une des régles précédentes.

## Exemple
```
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
```

En utilisant la méthode d'extension ou une instance de la class CompareurDeChaines les chaines
avec les indice correspondant vont retourner vrai lors de l'appele de la fonction Match.<br />
A = ["École", "Bureau", "Maison", "feuillet-inscription"]<br />
B = ["ecole", "bureau", "maison", "feuillet_inscription"]<br />

Avec des ensembles avec beaucoup de ressemblance, la méthode Match de la class CompareurDeChaines
va retouner vrai pour celui qui est le plus près seuelement.  Donc les chaines avec les indice 
correspondant vont retourner vrai lors de l'appele de la fonction Match.<br />
A = ["Frédéric", "frederic"]<br />
B = ["frédéric", "Frederic"]<br />

## Cas spéciaux
Lorsque les chaines sont écrit à la dure dans les DLL, s'ils sont identique, ils semble partager la
même instance. Ce qui fait que la comparaisons retourne de plusieurs valeur vrai pour un ensemble. 
Ce qui n'est pas le cas lorsque ils sont obtenu à partire de chaine non écrit dans le code.

Voir les méthodes de test :
- HardCodedVsStringObtenuParReflexion
- HardCodedVsStringObtenuParReflexion2