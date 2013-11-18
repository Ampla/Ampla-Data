using System.Dynamic;
using System.Linq;

namespace AmplaData.Dynamic.Methods.Strategies
{
    public class ArgumentMatchingStrategy
    {
        private readonly Argument[] arguments;
        public ArgumentMatchingStrategy(params Argument[] arguments)
        {
            this.arguments = arguments;
        }

        public bool Matches(InvokeMemberBinder binder, object[] args)
        {
            CallInfo callInfo = binder.CallInfo;
            return args.Length == arguments.Length && arguments.All(argument => argument.Matches(callInfo, args));
        }
    }
}