using System.Dynamic;

namespace AmplaData.Dynamic.Methods.Strategies
{
    /// <summary>
    ///     Strategy to match index binders and args.
    /// </summary>
    public interface IIndexStrategy
    {
        /// <summary>
        ///     Does the binder and Args match the index
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        bool Matches(GetIndexBinder binder, object[] args);
    }
}