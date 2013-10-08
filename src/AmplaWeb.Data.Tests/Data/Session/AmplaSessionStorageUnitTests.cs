using AmplaWeb.Data.Web.Wrappers;
using NUnit.Framework;

namespace AmplaWeb.Data.Session
{
    [TestFixture]
    public class AmplaSessionStorageUnitTests : TestFixture
    {
        private AmplaSessionStorage sessionStorage;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            sessionStorage = new AmplaSessionStorage(new SimpleHttpContext("http://localhost/").Session);
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