using System.Collections.Generic;
using AmplaWeb.Data.AmplaRepository;
using AmplaWeb.Data.Records;

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

        public AmplaRecord FindRecord(int id)
        {
            return repository.FindRecord(id);
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            return repository.FindByFilter(filters);
        }

        public AmplaAuditRecord GetHistory(int id)
        {
            return repository.GetHistory(id);
        }
    }
}