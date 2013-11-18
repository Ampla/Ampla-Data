using System.Dynamic;
using AmplaData.AmplaData2008;
using AmplaData.Dynamic.Methods.Binders;

namespace AmplaData.Dynamic.Methods.Strategies
{
    /// <summary>
    /// Strategy for Find By Id
    /// </summary>
    public class FindByIdStrategy : MemberStrategy
    {
        private static readonly ArgumentMatchingStrategy NamedIdArgument = new ArgumentMatchingStrategy(Argument.Named<int>("Id").IgnoreCase);
        private static readonly  ArgumentMatchingStrategy Position0Argument = new ArgumentMatchingStrategy(Argument.Position<int>(0));

        /// <summary>
        /// Gets the Dynamic binder for the Find and FindById dynamic methods
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public override IDynamicBinder GetBinder(InvokeMemberBinder binder, object[] args)
        {
            if (MethodCalled(binder, "Find"))
            {
                if (NamedIdArgument.Matches(binder, args) || Position0Argument.Matches(binder, args))
                {
                    return new FindByIdDynamicBinder(DataWebServiceFactory.Create(), CredentialsProvider.ForUsernameAndPassword("User", "password"));
                }
            }

            if (MethodCalled(binder, "FindById"))
            {
                if (Position0Argument.Matches(binder, args) || NamedIdArgument.Matches(binder, args))
                {
                    return new FindByIdDynamicBinder(DataWebServiceFactory.Create(), CredentialsProvider.ForUsernameAndPassword("User", "password"));
                }
            }
            return null;
        }
    }
}