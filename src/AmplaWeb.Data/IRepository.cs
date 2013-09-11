
namespace AmplaWeb.Data
{
    public interface IRepository<TModel> : IReadOnlyRepository<TModel>
    {
        void Add(TModel model);
        void Delete(TModel model);
        void Update(TModel model);
        void Confirm(TModel model);
        void Unconfirm(TModel model);
    }
}