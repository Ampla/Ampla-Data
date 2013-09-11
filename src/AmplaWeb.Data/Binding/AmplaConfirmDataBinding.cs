using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ViewData;


namespace AmplaWeb.Data.Binding
{
    public class AmplaConfirmDataBinding<TModel> : AmplaBinding where TModel : new()
    {
        private readonly List<TModel> models;
        private readonly List<UpdateRecordStatus> records;
        private readonly AmplaViewProperties<TModel> viewProperties;

        public AmplaConfirmDataBinding(List<TModel> models, List<UpdateRecordStatus> records, AmplaViewProperties<TModel> viewProperties)
        {
            this.models = models;
            this.records = records;
            this.viewProperties = viewProperties;
        }

        public override bool Bind()
        {
            throw new System.NotImplementedException();
        }
    }
}