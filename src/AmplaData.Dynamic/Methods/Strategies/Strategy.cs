using System.Dynamic;
using AmplaData.Dynamic.Methods.Binders;

namespace AmplaData.Dynamic.Methods.Strategies
{
    public abstract class Strategy : IStrategy
    {
        public abstract IDynamicBinder GetBinder(InvokeMemberBinder binder, object[] args);

        protected bool MethodCalled(InvokeMemberBinder binder, string name)
        {
            return binder.Name == name;
        }
    }
}