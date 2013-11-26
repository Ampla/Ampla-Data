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
                            return arg != null && CompareType(arg.GetType(), Type);
                        }
                    }
                }
                return false;
            }

            protected override string ToStringExtra()
            {
                return "Position: " + position;
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
                            return (arg != null) && (CompareType(arg.GetType(), Type));
                        }
                    }
                }
                return false;
            }

            protected override string ToStringExtra()
            {
                return "Named: " + name;
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

        protected bool CompareType(Type argType, Type requiredType)
        {
            return argType == requiredType || requiredType.IsAssignableFrom(argType);
        }

        public abstract bool Matches(CallInfo callInfo, object[] args);

        protected abstract string ToStringExtra();

        public override string ToString()
        {
            string toString = string.Format("Argument (Type: {0}, {1})", Type.FullName, ToStringExtra());
            return toString;
        }
    }
}