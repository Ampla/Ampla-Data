using System;

namespace AmplaWeb.Data.Binding.MetaData
{
    public static class DataTypeHelper
    {
         public static Type GetDataType(string amplaDataType)
         {
             switch (amplaDataType)
             {
                 case "xs:Int":
                     {
                         return typeof (int);
                     }
                 case "xs:DateTime":
                     {
                         return typeof(DateTime);
                     }

                 case "xs:Boolean":
                     {
                         return typeof(bool);
                     }
                 case "xs:String":
                     {
                         return typeof(string);
                     }
                 default:
                     {
                         throw new ArgumentOutOfRangeException("amplaDataType", amplaDataType, amplaDataType + " is not currently supported.");
                     }
             }
         }
    }
}