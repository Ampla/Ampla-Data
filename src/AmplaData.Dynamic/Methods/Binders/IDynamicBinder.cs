using System.Dynamic;

namespace AmplaData.Dynamic.Methods.Binders
{
    public interface IDynamicBinder
    {
        dynamic Invoke(DynamicViewPoint viewPoint, InvokeMemberBinder binder, object[] args);
    }
}