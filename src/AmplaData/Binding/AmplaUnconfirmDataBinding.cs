using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.ModelData;

namespace AmplaData.Binding
{
    public class AmplaUnconfirmDataBinding<TModel> : AmplaUpdateRecordStatusBinding<TModel> where TModel : new()
    {
        public AmplaUnconfirmDataBinding(List<TModel> models, List<UpdateRecordStatus> records, IModelProperties<TModel> modelProperties)
            : base(models, records, modelProperties, UpdateRecordStatusAction.Unconfirm)
        {
        }
    }
}