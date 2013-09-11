using System.Collections.Generic;

namespace AmplaWeb.Data.Binding.ViewData
{
    public class ViewCollection<T>
    {
        private readonly List<T> list = new List<T>();
        private readonly Dictionary<string, T> dictionary = new Dictionary<string, T>();

        protected void Add(string key, T value)
        {
            list.Add(value);
            dictionary[key] = value;
        }

        public IEnumerable<T> GetValues()
        {
            return list;
        }
    }
}
