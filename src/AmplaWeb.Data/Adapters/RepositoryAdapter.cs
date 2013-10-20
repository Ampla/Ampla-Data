using System.Collections.Generic;
using AmplaWeb.Data.Records;

namespace AmplaWeb.Data.Adapters
{
    public abstract class RepositoryAdapter<TModel> : IRepository<TModel>
    {
        private readonly IRepository<TModel> repository;

        protected RepositoryAdapter(IRepository<TModel> repository)
        {
            this.repository = repository;
        }

        protected abstract void Adapt();

        public void Dispose()
        {
            repository.Dispose();
        }

        public IList<TModel> GetAll()
        {
            Adapt();
            return repository.GetAll();
        }

        public TModel FindById(int id)
        {
            Adapt();
            return repository.FindById(id);
        }

        public AmplaRecord FindRecord(int id)
        {
            Adapt();
            return repository.FindRecord(id);
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            Adapt();
            return repository.FindByFilter(filters);
        }

        public AmplaAuditRecord GetHistory(int id)
        {
            Adapt();
            return repository.GetHistory(id);
        }

        public void Add(TModel model)
        {
            Adapt();
            repository.Add(model);
        }

        public void Delete(TModel model)
        {
            Adapt();
            repository.Delete(model);
        }

        public void Update(TModel model)
        {
            Adapt();
            repository.Update(model);
        }

        public void Confirm(TModel model)
        {
            Adapt();
            repository.Confirm(model);
        }

        public void Unconfirm(TModel model)
        {
            Adapt();
            repository.Unconfirm(model);
        }

        public List<string> GetAllowedValues(string property)
        {
            Adapt();
            return repository.GetAllowedValues(property);
        }
    }
}