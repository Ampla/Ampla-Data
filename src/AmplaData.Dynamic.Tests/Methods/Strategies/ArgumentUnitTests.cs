using System.Dynamic;
using NUnit.Framework;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public class ArgumentUnitTests : TestFixture
    {
        [Test]
        public void NamedArgumentToString()
        {
            Argument argument = Argument.Named<int>("Id");
            string toString = argument.ToString();

            StringAssert.Contains("Id", toString);
            StringAssert.Contains(typeof(int).Name, toString);
        }

        [Test]
        public void PostionArgumentToString()
        {
            Argument argument = Argument.Position<int>(10);
            string toString = argument.ToString();

            StringAssert.Contains("10", toString);
            StringAssert.Contains(typeof(int).Name, toString);
        }

        [Test]
        public void OneNamedArgument()
        {
            Argument argument = Argument.Named<int>("Id");

            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1, "Id");
            Assert.That(argument.Matches(binder.CallInfo, new object[] {100}), Is.True);
        }

        [Test]
        public void NamedArgumentIgnoreCase()
        {
            Argument argument = Argument.Named<int>("id").IgnoreCase;
            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1, "Id");
            Assert.That(argument.Matches(binder.CallInfo, new object[] { 100 }), Is.True);
        }

        [Test]
        public void OnePositionalArgument()
        {
            Argument argument = Argument.Position<int>(0);
            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1);
            Assert.That(argument.Matches(binder.CallInfo, new object[] { 100 }), Is.True);
        }

        [Test]
        public void IncorrectNamedArgument()
        {
            Argument argument = Argument.Named<int>("SetId");
            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1, "Id");
            Assert.That(argument.Matches(binder.CallInfo, new object[] { 100 }), Is.False);
        }

        [Test]
        public void DynamicArgument()
        {
            Argument argument = Argument.Position<dynamic>(0);
            InvokeMemberBinder binder = Binder.GetMemberBinder("Save", 1);
            dynamic model = new {Location = "Enterprise.Site.Area.Production", Value = 100};
            Assert.That(argument.Matches(binder.CallInfo, new object[] {model}), "Argument: {0}", argument);
        }
    }
}