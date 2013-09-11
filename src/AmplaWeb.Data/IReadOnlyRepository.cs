using System;
using System.Collections.Generic;
using AmplaWeb.Data.AmplaRespository;

namespace AmplaWeb.Data
{
    public interface IReadOnlyRepository<TModel> : IDisposable
    {
        IList<TModel> GetAll();
        TModel FindById(int id);
        IList<TModel> FindByFilter(params FilterValue[] filters);
    }
}