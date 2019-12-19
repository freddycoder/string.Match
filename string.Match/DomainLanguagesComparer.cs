using System.Collections.Generic;
using System.Linq;

namespace @string.Match
{
    public interface IDomainLanguagesComparer<T>
    {
        T this[string name] { get; }
    }

    public class DomainLanguagesComparer<T> : IDomainLanguagesComparer<T>
    {
        private List<HashSet<string>> _keys;
        private List<T> _values;

        public T this[string name] => Find(name);

        private T Find(string name)
        {
            int index = _keys.IndexOf(k => k.Contains(name));

            if (index != -1)
            {
                return _values[index];
            }

            return default;
        }

        public DomainLanguagesComparer(IEnumerable<T> items)
        {
            InitMembers(items.Count(), typeof(T).GetProperties().Length);

            items.ForEach((item, i) =>
            {
                typeof(T).GetProperties().ForEach(property =>
                {
                    _keys[i].Add(property.GetValue(item) as string);
                });

                _values[i] = item;
            });
        }

        private void InitMembers(int nbItems, int length)
        {
            _keys = new List<HashSet<string>>(nbItems);

            for (int i = 0; i < nbItems; i++)
            {
                _keys.Add(new HashSet<string>(length, new CompareurDeChaines()));
            }

            _values = new List<T>(nbItems);
        }
    }
}
