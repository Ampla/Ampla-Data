using System;
using System.Collections.Generic;
using System.Dynamic;
using AmplaData.Binding.MetaData;
using AmplaData.Dynamic.Methods.Strategies;

namespace AmplaData.Dynamic
{
    public class DynamicRecord : DynamicObject, IRecordLoad
    {
        private readonly Dictionary<string, Type> columns = new Dictionary<string, Type>();
        
        private readonly Dictionary<string, object> values = new Dictionary<string, object>(); 

        public DynamicRecord(string module)
        {
            Module = module;
        }

        public string Module { get; private set; }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            IndexStrategy indexMatchingStrategy = IndexStrategy.ForStringIndex();
            if (indexMatchingStrategy.Matches(binder, indexes))
            {
                string field = (string)indexes[0];
                return values.TryGetValue(field, out result);
            }
            return base.TryGetIndex(binder, indexes, out result);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return base.TrySetIndex(binder, indexes, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string field = binder.Name;

            return values.TryGetValue(field, out result) 
                || base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return base.TrySetMember(binder, value);
        }
        
        void IRecordLoad.AddColumn(string field, Type dataType)
        {
            columns.Add(field, dataType);
            values.Add(field, DataTypeHelper.GetDefaultValue(dataType));
        }

        void IRecordLoad.SetValue(string field, string invariantValue)
        {
            Type dataType;

            if (!columns.TryGetValue(field, out dataType))
            {
                throw new ArgumentException(field + " is not a valid field");
            }
            object value = Convert.ChangeType(invariantValue, dataType);
            values[field] = value;
        }
    }
}