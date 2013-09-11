using System.Collections.Generic;

namespace AmplaWeb.Data.InMemory
{
    public class ReadOnlyRepository<TModel> : IReadOnlyRepository<TModel>
    {
        private IRepository<TModel> repository;

        public ReadOnlyRepository(IRepository<TModel> repository)
        {
            this.repository = repository;
        }

        public void Dispose()
        {
            repository = null;
        }

        public IList<TModel> GetAll()
        {
            return repository.GetAll();
        }

        public TModel FindById(int id)
        {
            return repository.FindById(id);
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            return repository.FindByFilter(filters);
        }
    }
}