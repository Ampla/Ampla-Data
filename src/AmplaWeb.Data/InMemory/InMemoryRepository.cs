using System;
using System.Collections.Generic;
using AmplaData.Data.Binding.ModelData;
using AmplaData.Data.Records;

namespace AmplaData.Data.InMemory
{
    public class InMemoryRepository<TModel> : IRepository<TModel> where TModel : new()
    {
        private List<TModel> models;

        public InMemoryRepository()
        {
            models = new List<TModel>();
        }

        public void Dispose()
        {
            models = null;
        }

        public IList<TModel> GetAll()
        {
            return models.ToArray();
        }

        public TModel FindById(int id)
        {
            return models.Find(m => ModelIdentifier.GetValue<TModel, int>(m) == id);
        }

        public AmplaRecord FindRecord(int id)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            List<TModel> list = new List<TModel>(models);

            foreach (FilterValue filterValue in filters)
            {
                if (list.Count > 0)
                {
                    string property = filterValue.Name;
                    string value = filterValue.Value;
                    list = list.FindAll(m => Property<TModel>.GetValue<string>(m, property) == value);
                }
            }
            return list;
        }

        public AmplaAuditRecord GetHistory(int id)
        {
            throw new NotImplementedException();
        }

        public ModelVersions GetVersions(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(TModel model)
        {
            int id = ModelIdentifier.GetValue<TModel, int>(model);
            if (id == 0)
            {
                int newId = models.Count + 1;
                ModelIdentifier.SetValue(model, newId);
            }
            models.Add(model);
        }

        public void Delete(TModel model)
        {
            models.Remove(model);
        }

        public void Confirm(TModel model)
        {
            throw new NotImplementedException();
        }

        public void Unconfirm(TModel model)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllowedValues(string property)
        {
            throw new NotImplementedException();
        }

        public void Update(TModel model)
        {
            TModel prev = FindById(ModelIdentifier.GetValue<TModel, int>(model));
            Delete(prev);
            Add(model);
        }
    }
}