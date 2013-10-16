using System.Collections.Generic;

namespace AmplaWeb.Data
{
    public class ReadOnlyRepository<TModel> : IReadOnlyRepository<TModel>
    {
        private readonly IRepository<TModel> repository;

        public ReadOnlyRepository(IRepository<TModel> repository)
        {
            this.repository = repository;
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        public IList<TModel> GetAll()
        {
            return repository.GetAll();
        }

        public TModel FindById(int id)
        {
            return repository.FindById(id);
        }

        public dynamic FindRecord(int id)
        {
            return repository.FindRecord(id);
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            return repository.FindByFilter(filters);
        }
    }
}