namespace AmplaWeb.Data
{
    public interface IRepositorySet
    {
        IRepository<TModel> GetRepository<TModel>() where TModel : class, new();
        IReadOnlyRepository<TModel> GetReadOnlyRepository<TModel>() where TModel : class, new();
    }
}