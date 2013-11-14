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
                new DataTypeMap(typeof (int), "xs:Int"),
                new DataTypeMap(typeof (string), "xs:String"),
                new DataTypeMap(typeof (bool), "xs:Boolean"),
                new DataTypeMap(typeof (DateTime), "xs:DateTime"),
                new DataTypeMap(typeof (Double), "xs:Double"),
                new DataTypeMap(typeof (Single), "xs:Single"),
                new DataTypeMap(typeof(byte), "xs:Byte"),
            };

            AmplaToTypeDictionary = new Dictionary<string, Type>();
            TypeToAmplaDictionary = new Dictionary<Type, string>();

            foreach (DataTypeMap map in maps)
            {
                AmplaToTypeDictionary[map.AmplaType] = map.DataType;
                TypeToAmplaDictionary[map.DataType] = map.AmplaType;
            }
        }

        public class DataTypeMap
        {
            public DataTypeMap(Type type, string amplaType)
            {
                DataType = type;
                AmplaType = amplaType;
            }

            public Type DataType { get; private set; }
            public string AmplaType { get; private set; }
        }

        private static readonly Dictionary<string, Type> AmplaToTypeDictionary ;
        private static readonly Dictionary<Type, string> TypeToAmplaDictionary ;

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
    }
}