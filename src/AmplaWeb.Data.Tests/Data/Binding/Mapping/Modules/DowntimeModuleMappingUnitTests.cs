using AmplaWeb.Data.Binding.ViewData;
using AmplaWeb.Data.Downtime;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    [TestFixture]
    public class DowntimeModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public DowntimeModuleMappingUnitTests() : base(DowntimeViews.StandardView, ()=> new DowntimeModuleMapping())
        {
        }

        [Test]
        public void IdField()
        {
            CheckField<IdFieldMapping>("Id", "Id", true, false);
        }
        
        [Test]
        public void LocationField()
        {
            CheckField<ReadOnlyFieldMapping>("ObjectId", "Location", true, false);
        }

        [Test]
        public void StartDateTime()
        {
            CheckField<DefaultValueFieldMapping>("StartDateTime", "Start Time", true, true);
        }

        [Test]
        public void EndDateTime()
        {
            CheckField<ModelFieldMapping>("EndDateTime", "End Time", true, false);
        }

        [Test]
        public void CauseLocation()
        {
            CheckField<ValidatedModelFieldMapping>("Cause Location", "Cause Location", true, false);
        }

        [Test]
        public void Cause()
        {
            CheckField<ValidatedModelFieldMapping>("Cause", "Cause", true, false);
        }

        [Test]
        public void Classification()
        {
            CheckField<ValidatedModelFieldMapping>("Classification", "Classification", true, false);
        }

 
    }
}