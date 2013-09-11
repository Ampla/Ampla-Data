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
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaModuleAttribute"/> class.
        /// </summary>
        public AmplaModuleAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplaModuleAttribute"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        public AmplaModuleAttribute(string module)
        {
            Module = module;
        }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        public string Module { get; set; }

        /// <summary>
        ///     Tries to get the Ampla module from the specified type. 
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="amplaModule">The ampla module.</param>
        /// <returns></returns>
        public static bool TryGetModule<TModel>(out AmplaModules? amplaModule)
        {
            Type type = typeof (TModel);
            while (type != null && type != typeof (object))
            {
                AmplaModuleAttribute attribute;
                if (type.TryGetAttribute(out attribute))
                {
                    AmplaModules module;
                    if (Enum.TryParse(attribute.Module, out module))
                    {
                        amplaModule = module;
                        return true;
                    }
                }
                type = type.BaseType;
            }
            amplaModule = null;
            return false;
        }
    }
}