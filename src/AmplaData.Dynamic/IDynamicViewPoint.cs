using System.Collections.Generic;
using System.Collections.Specialized;

namespace AmplaData.Dynamic
{
    public interface IDynamicViewPoint
    {
        dynamic CreateFrom(NameValueCollection collection);

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        dynamic Save(object model);

        dynamic Insert(object model);

        dynamic Update(object model);

        string Location { get; }

        string Module { get; }
    }
}