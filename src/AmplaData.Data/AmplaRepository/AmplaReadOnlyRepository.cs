
namespace AmplaData.AmplaRepository
{
    /// <summary>
    ///     The Ampla ReadOnly Repository allows the reading of Ampla models
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class AmplaReadOnlyRepository<TModel> : ReadOnlyRepository<TModel> where TModel : class, new()
    {
        public AmplaReadOnlyRepository(AmplaRepository<TModel> repository) : base(repository)
        {
        }
    }
}