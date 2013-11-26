using System.Dynamic;

namespace AmplaData.Dynamic.Methods.Strategies
{
    public class IndexStrategy : IIndexStrategy
    {
        /// <summary>
        /// Creates an Index Strategy for a single string indexer
        /// </summary>
        /// <returns></returns>
        public static IndexStrategy ForStringIndex()
        {
            return new IndexStrategy(Argument.Position<string>(0));
        }

        private readonly Argument argument;

        private IndexStrategy(Argument argument)
        {
            this.argument = argument;
        }

        public bool Matches(GetIndexBinder binder, object[] args)
        {
            CallInfo callInfo = binder.CallInfo;
            return callInfo.ArgumentCount == 1 && argument.Matches(callInfo, args);
        }
    }
}