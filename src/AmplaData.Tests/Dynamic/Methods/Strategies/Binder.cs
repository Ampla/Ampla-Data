using System;
using System.Dynamic;

namespace AmplaData.Dynamic.Methods.Strategies
{
    public class Binder
    {
        public static InvokeMemberBinder GetMemberBinder(string name, int argCount, params string[] namedArgs)
        {
            CallInfo callInfo = new CallInfo(argCount, namedArgs);
            return new TestInvokeMemberBinder(name, callInfo);
        }

        public static GetIndexBinder GetIndexBinder(int argCount, params string[] namedArgs)
        {
            CallInfo callInfo = new CallInfo(argCount, namedArgs);
            return new TestGetIndexBinder(callInfo);
        }

        private class TestInvokeMemberBinder : InvokeMemberBinder
        {
            public TestInvokeMemberBinder(string name, CallInfo callInfo) : base(name, false, callInfo)
            {
            }

            public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args,
                                                                   DynamicMetaObject errorSuggestion)
            {
                throw new NotImplementedException();
            }

            public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args,
                                                             DynamicMetaObject errorSuggestion)
            {
                throw new NotImplementedException();
            }
        }

        private class TestGetIndexBinder : GetIndexBinder
        {
            public TestGetIndexBinder(CallInfo callInfo) : base(callInfo)
            {
            }

            public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes,
                                                               DynamicMetaObject errorSuggestion)
            {
                throw new NotImplementedException();
            }
        }
    }
}