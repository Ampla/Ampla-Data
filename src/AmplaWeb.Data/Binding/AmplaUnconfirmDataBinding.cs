﻿using System.Collections.Generic;
using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Binding.ViewData;

namespace AmplaWeb.Data.Binding
{
    public class AmplaUnconfirmDataBinding<TModel> : AmplaBinding where TModel : new()
    {
        private List<TModel> models;
        private List<UpdateRecordStatus> records;
        private AmplaViewProperties<TModel> viewProperties;

        public AmplaUnconfirmDataBinding(List<TModel> models, List<UpdateRecordStatus> records, AmplaViewProperties<TModel> viewProperties)
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