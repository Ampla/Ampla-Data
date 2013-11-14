using System.Collections.Generic;
using AmplaData.AmplaData2008;

namespace AmplaData.Binding.Mapping.Modules
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
                {AmplaModules.Energy, new EnergyModuleMapping()},
                {AmplaModules.Inventory, new NullModuleMapping()},
                {AmplaModules.Knowledge, new KnowledgeModuleMapping()},
                {AmplaModules.Maintenance, new MaintenanceModuleMapping()},
                {AmplaModules.Metrics, new MetricsModuleMapping()},
                {AmplaModules.Planning, new PlanningModuleMapping()},
                {AmplaModules.Production, new ProductionModuleMapping()},
                {AmplaModules.Quality, new QualityModuleMapping()},
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