using System;
using System.Dynamic;

namespace AmplaData.Dynamic.Methods.Strategies
{
    public abstract class Argument
    {
        public class PositionalArgument : Argument
        {
            private readonly int position;

            public PositionalArgument(int position, Type type) : base(type)
            {
                this.position = position;
            }

            public override bool Matches(CallInfo callInfo, object[] args)
            {
                if (callInfo.ArgumentCount > position)
                {
                    string argName = callInfo.ArgumentNames.Count > position
                                         ? callInfo.ArgumentNames[position]
                                         : null;

                    if (string.IsNullOrEmpty(argName))
                    {
                        if (args.Length > position)
                        {
                            object arg = args[position];
                            return arg != null && arg.GetType() == Type;
                        }
                    }
                }
                return false;
            }

        }

        public class NamedArgument : Argument
        {
            private readonly string name;
            private bool ignoreCase;

            public NamedArgument(string name, Type type)
                : base(type)
            {
                this.name = name;
            }

            public Argument IgnoreCase
            {
                get
                {
                    ignoreCase = true;
                    return this;
                }
            }

            public override bool Matches(CallInfo callInfo, object[] args)
            {
                StringComparer comparer = ignoreCase
                                              ? StringComparer.InvariantCultureIgnoreCase
                                              : StringComparer.InvariantCulture;

                for (int i = 0; i < callInfo.ArgumentNames.Count; i++)
                {
                    string argName = callInfo.ArgumentNames[i];
                    if (comparer.Compare(argName, name) == 0)
                    {
                        if (i <= args.Length)
                        {
                            object arg = args[i];
                            return (arg != null) && (arg.GetType() == Type);
                        }
                    }
                }
                return false;
            }
        }
        
        protected Type Type { get; private set; }

        protected Argument(Type type)
        {
            Type = type;
        }

        public static NamedArgument Named<T>(string name)
        {
            NamedArgument argument = new NamedArgument(name, typeof (T));
            return argument;
        }

        public static PositionalArgument Position<T>(int position)
        {
            return new PositionalArgument(position, typeof (T));
        }

        public abstract bool Matches(CallInfo callInfo, object[] args);
    }
}