using System.Collections.Generic;
using AmplaData.Data.AmplaData2008;
using AmplaData.Data.Binding.ModelData;

namespace AmplaData.Data.Binding
{
    public class AmplaUnconfirmDataBinding<TModel> : AmplaUpdateRecordStatusBinding<TModel> where TModel : new()
    {
        public AmplaUnconfirmDataBinding(List<TModel> models, List<UpdateRecordStatus> records, IModelProperties<TModel> modelProperties)
            : base(models, records, modelProperties, UpdateRecordStatusAction.Unconfirm)
        {
        }
    }
}