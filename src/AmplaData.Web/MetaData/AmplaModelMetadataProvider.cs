using System.Diagnostics;
using System.Web.Mvc;

namespace AmplaData.Web.MetaData
{
    public class AmplaModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(System.Collections.Generic.IEnumerable<System.Attribute> attributes, System.Type containerType, System.Func<object> modelAccessor, System.Type modelType, string propertyName)
        {
            Debug.WriteLine("CreateMetadata for {0} {1}", modelType, propertyName);
            return base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
        }
    }
}