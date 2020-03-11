using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace @string.Match
{
    public class MultiKeyReadOnlyDictionnary<T> : IReadOnlyDictionary<string, T>
    {
        private List<HashSet<string>> _keys;
        private List<T> _values;

        public IEnumerable<string> Keys 
        {
            get
            {
                foreach (var keySet in _keys)
                {
                    foreach (var key in keySet)
                    {
                        yield return key;
                    }
                }
            }
        }

        public IEnumerable<T> Values => _values;

        public int Count => _values.Count;

        public T this[string key]
        {
            get
            {
                int index = _keys.IndexOf(k => k.Contains(key));

                if (index != -1)
                {
                    return _values[index];
                }

                throw new KeyNotFoundException($"Cannot found any match for key {key}");
            }
        }

        public MultiKeyReadOnlyDictionnary(IEnumerable<T> items)
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

        public bool ContainsKey(string key)
        {
            int index = _keys.IndexOf(k => k.Contains(key));

            return index != -1;
        }

        public bool TryGetValue(string key, out T value)
        {
            try
            {
                value = this[key];

                return true;
            }
            catch
            {
                value = default;

                return false;
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                foreach (var key in _keys[i])
                {
                    yield return new KeyValuePair<string, T>(key, _values[i]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
