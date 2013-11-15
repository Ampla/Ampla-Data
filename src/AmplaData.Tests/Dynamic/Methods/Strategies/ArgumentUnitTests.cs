using System.Dynamic;
using NUnit.Framework;

namespace AmplaData.Dynamic.Methods.Strategies
{
    [TestFixture]
    public class ArgumentUnitTests : TestFixture
    {
        [Test]
        public void OneNamedArgument()
        {
            Argument argument = Argument.Named<int>("Id");

            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1, "Id");
            
            Assert.That(argument.Matches(binder, new object[] {100}), Is.True);
        }

        [Test]
        public void NamedArgumentIgnoreCase()
        {
            Argument argument = Argument.Named<int>("id").IgnoreCase;
            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1, "Id");
            Assert.That(argument.Matches(binder, new object[] { 100 }), Is.True);
        }

        [Test]
        public void OnePositionalArgument()
        {
            Argument argument = Argument.Position<int>(0);
            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1);
            Assert.That(argument.Matches(binder, new object[] { 100 }), Is.True);
        }

        [Test]
        public void IncorrectNamedArgument()
        {
            Argument argument = Argument.Named<int>("SetId");
            InvokeMemberBinder binder = Binder.GetMemberBinder("Find", 1, "Id");
            Assert.That(argument.Matches(binder, new object[] { 100 }), Is.False);
        }
    }
}