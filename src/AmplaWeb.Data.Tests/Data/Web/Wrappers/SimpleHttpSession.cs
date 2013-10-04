using System.Collections.Generic;
using AmplaWeb.Data.Web.Interfaces;

namespace AmplaWeb.Data.Web.Wrappers
{
    public class SimpleHttpSession : IHttpSessionWrapper
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();
        public void SetValue(string key, object value)
        {
            dictionary[key] = value;
        }

        public object GetValue(string key)
        {
            return dictionary[key];
        }
    }
}