using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.ModelData;
using AmplaData.Binding.ModelData.Validation;

namespace AmplaData.Dynamic.Binding.ModelData
{
    public class DynamicModelProperties : IDynamicModelProperties
    {
        private readonly IDynamicViewPoint viewPoint;

        public DynamicModelProperties(IDynamicViewPoint viewPoint)
        {
            this.viewPoint = viewPoint;
        }

        public string GetLocation(object model)
        {
            return viewPoint.Location;
        }

        public LocationFilter LocationFilter
        {
            get { return new LocationFilter(viewPoint.Location, false); }
        }

        public FilterValue[] DefaultFilters
        {
            get { return new FilterValue[0]; }
        }

        public AmplaModules Module
        {
            get
            {
                AmplaModules module;
                Enum.TryParse(viewPoint.Module, out module);
                return module;
            }
        }

        public bool ResolveIdentifiers
        {
            get { return viewPoint.Module == "Downtime"; }
        }

        public bool TrySetValueFromString(object model, string propertyName, string value)
        {
            IRecordLoad recordLoad = model as IRecordLoad;
            if (recordLoad != null)
            {
                recordLoad.SetValue(propertyName, value);
                return true;
            }
            return false;
        }

        public IList<string> GetProperties()
        {
            return new List<string>();
        }

        public bool TryGetPropertyValue(object model, string propertyName, out string value)
        {
            IDictionary<string, object> dictionary = model as IDictionary<string, object>;
            if (dictionary != null)
            {
                object propValue;
                if (dictionary.TryGetValue(propertyName, out propValue))
                {
                    value = Convert.ToString(propValue);
                    return true;
                }
            }
            value = null;
            return false;
        }

        public bool IsDefaultValue(object model, string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool ValidateModel(object model, ValidationMessages validationMessages)
        {
            return true;
        }

        public object CloneModel(object model)
        {
            throw new NotImplementedException();
        }

        public string GetModelName()
        {
            return viewPoint.Module;
        }
    }
}