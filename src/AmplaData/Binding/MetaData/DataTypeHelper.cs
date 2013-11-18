using System;
using System.Collections.Generic;

namespace AmplaData.Binding.MetaData
{
    public static class DataTypeHelper
    {
        static DataTypeHelper()
        {
            List<DataTypeMap> maps = new List<DataTypeMap>
            {
                new DataTypeMap(typeof (int), "xs:Int", 0),
                new DataTypeMap(typeof (string), "xs:String", null),
                new DataTypeMap(typeof (bool), "xs:Boolean", false),
                new DataTypeMap(typeof (DateTime), "xs:DateTime", DateTime.MinValue),
                new DataTypeMap(typeof (double), "xs:Double", 0D),
                new DataTypeMap(typeof (float), "xs:Single", 0F),
                new DataTypeMap(typeof(byte), "xs:Byte", (byte)0),
            };

            AmplaToTypeDictionary = new Dictionary<string, Type>();
            TypeToAmplaDictionary = new Dictionary<Type, string>();
            DefaultValueDictionary =new Dictionary<Type, object>();

            foreach (DataTypeMap map in maps)
            {
                AmplaToTypeDictionary[map.AmplaType] = map.DataType;
                TypeToAmplaDictionary[map.DataType] = map.AmplaType;
                DefaultValueDictionary[map.DataType] = map.DefaultValue;
            }
        }

        private class DataTypeMap
        {
            public DataTypeMap(Type type, string amplaType, object defaultValue)
            {
                DataType = type;
                AmplaType = amplaType;
                DefaultValue = defaultValue;
            }

            public Type DataType { get; private set; }
            public string AmplaType { get; private set; }
            public object DefaultValue { get; private set; }
        }

        private static readonly Dictionary<string, Type> AmplaToTypeDictionary ;
        private static readonly Dictionary<Type, string> TypeToAmplaDictionary ;

        private static readonly Dictionary<Type, object> DefaultValueDictionary; 

        public static Type GetDataType(string amplaDataType)
        {
            Type dataType;
            if (AmplaToTypeDictionary.TryGetValue(amplaDataType, out dataType))
            {
                return dataType;
            }
            throw new ArgumentOutOfRangeException("amplaDataType", amplaDataType, "Ampla DataType is not mapped");
        }

        public static string GetAmplaDataType(Type dataType)
        {
            string amplaType;
            if (TypeToAmplaDictionary.TryGetValue(dataType, out amplaType))
            {
                return amplaType;
            }
            throw new ArgumentOutOfRangeException("dataType", dataType, "DataType is not mapped");
        }

        public static string GetAmplaDataType<T>()
        {
            return GetAmplaDataType(typeof (T));
        }

        public static object GetDefaultValue(Type dataType)
        {
            object defaultValue;
            if (DefaultValueDictionary.TryGetValue(dataType, out defaultValue))
            {
                return defaultValue;
            }
            return null;
        }
    }
}