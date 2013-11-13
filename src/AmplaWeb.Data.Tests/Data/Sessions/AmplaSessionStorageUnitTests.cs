using AmplaData.Data.Web.Wrappers;
using NUnit.Framework;

namespace AmplaData.Data.Sessions
{
    [TestFixture]
    public class AmplaSessionStorageUnitTests : TestFixture
    {
        private AmplaSessionStorage sessionStorage;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            SimpleHttpContext context = SimpleHttpContext.Create("http://localhost");
            sessionStorage = new AmplaSessionStorage(context.Session);
        }
        
        [Test]
        public void DefaultSession()
        {
            Assert.That(sessionStorage.GetAmplaSession(), Is.Empty);
        }

        [Test]
        public void GetAmplaSession()
        {
            Assert.That(sessionStorage.GetAmplaSession(), Is.Empty);

            sessionStorage.SetAmplaSession("New Session");
            Assert.That(sessionStorage.GetAmplaSession(), Is.EqualTo("New Session"));
        }
    }
}