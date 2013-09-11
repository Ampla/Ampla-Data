using System;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.MetaData;

namespace AmplaWeb.Data.Attributes
{
    /// <summary>
    ///     Attribute used to denote the module that is used for Ampla Model
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AmplaModuleAttribute : Attribute
    {
        public string Module { get; set; }

        /// <summary>
        ///     Tries to get the Ampla module from the specified type. 
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="amplaModule">The ampla module.</param>
        /// <returns></returns>
        public static bool TryGetModule<TModel>(out AmplaModules amplaModule)
        {
            AmplaModuleAttribute attribute;
            if (typeof(TModel).TryGetAttribute(out attribute))
            {
                Enum.TryParse(attribute.Module, out amplaModule);
                return true;
            }
            amplaModule = AmplaModules.Downtime;
            return false;
        }
    }
}