using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;

namespace AmplaData.Dynamic
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Turns the object into an ExpandoObject
        /// </summary>
        public static dynamic ToExpando(this object o)
        {
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary
            if (o is ExpandoObject) return o; //shouldn't have to... but just in case
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>()
                    .Select(key => new KeyValuePair<string, object>(key, nv[key]))
                    .ToList()
                    .ForEach(d.Add);
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }
    }

}