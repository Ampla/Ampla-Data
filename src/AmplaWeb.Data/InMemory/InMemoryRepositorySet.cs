using System;
using System.Collections.Generic;

namespace AmplaData.Data.InMemory
{
    public class InMemoryRepositorySet : IRepositorySet
    {
        private readonly Dictionary<Type, object> sets = new Dictionary<Type, object>();

        public IRepository<TModel> GetRepository<TModel>() where TModel : class, new()
        {
            object repository;

            Type modelType = typeof (TModel);

            if (!sets.TryGetValue(modelType, out repository))
            {
                repository = new InMemoryRepository<TModel>();
                sets[modelType] = repository;
            }
            return (IRepository<TModel>) repository;
        }

        public IReadOnlyRepository<TModel> GetReadOnlyRepository<TModel>() where TModel : class, new()
        {
            IRepository<TModel> repository = GetRepository<TModel>();
            return new ReadOnlyRepository<TModel>(repository);
        }
    }
}
