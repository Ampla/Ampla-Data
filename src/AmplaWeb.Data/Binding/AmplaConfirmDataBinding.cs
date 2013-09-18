using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ModelData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaConfirmDataBinding<TModel> : AmplaUpdateRecordStatusBinding<TModel> where TModel : new()
    {
        public AmplaConfirmDataBinding(List<TModel> models, List<UpdateRecordStatus> records, IModelProperties<TModel> modelProperties) 
            : base(models, records, modelProperties, UpdateRecordStatusAction.Confirm)
        {
        }
    }
}