using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    /// <summary>
    ///     Mapping lookup for specific Field Mappings per module
    /// </summary>
    public class ModuleMapping 
    {
        private class NullModuleMapping : StandardModuleMapping
        {
        }

        private static readonly Dictionary<AmplaModules, IModuleMapping> ModuleMappings = new Dictionary
            <AmplaModules, IModuleMapping>
            {
                {AmplaModules.Cost, new NullModuleMapping()},
                {AmplaModules.Downtime, new DowntimeModuleMapping()},
                {AmplaModules.Energy, new NullModuleMapping()},
                {AmplaModules.Inventory, new NullModuleMapping()},
                {AmplaModules.Knowledge, new KnowledgeModuleMapping()},
                {AmplaModules.Maintenance, new NullModuleMapping()},
                {AmplaModules.Metrics, new NullModuleMapping()},
                {AmplaModules.Planning, new NullModuleMapping()},
                {AmplaModules.Production, new ProductionModuleMapping()},
                {AmplaModules.Quality, new NullModuleMapping()},
            };

        /// <summary>
        /// Gets the module mapping for the specified module
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns></returns>
         public static IModuleMapping GetModuleMapping(AmplaModules module)
         {
             IModuleMapping moduleMapping;
             ModuleMappings.TryGetValue(module, out moduleMapping);
             
             return moduleMapping;
         }

    }
}