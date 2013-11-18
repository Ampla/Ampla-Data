using System;
using System.Collections.Generic;
using System.Dynamic;
using AmplaData.AmplaData2008;
using AmplaData.Dynamic.Methods.Binders;
using AmplaData.Dynamic.Methods.Strategies;

namespace AmplaData.Dynamic
{
    public class DynamicViewPoint : DynamicObject
    {
        public DynamicViewPoint(string location = "", string module = "")
        {
            Module = module;
            Location = location;
        }

        public string Module { get; set; }
        public string Location { get; set; }

        public AmplaModules AmplaModule
        {
            get
            {
                AmplaModules amplaModule;
                Enum.TryParse(Module, out amplaModule);
                return amplaModule;
            }
        }

        /// <summary>
        ///     Try to Invoke a dynamic method 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            List<IMemberStrategy> strategies = GetStrategies(this);

            foreach (var strategy in strategies)
            {
                IDynamicBinder dynamicBinder = strategy.GetBinder(binder, args);
                if (dynamicBinder != null)
                {
                    result = dynamicBinder.Invoke(this, binder, args);
                    return true;
                }
            }
            result = null;
            return false;
        }


        protected static List<IMemberStrategy> GetStrategies(DynamicViewPoint point)
        {
            return new List<IMemberStrategy> { new FindByIdStrategy() };
        }
    }
}
