﻿using AmplaWeb.Data.AmplaData2008;
using AmplaWeb.Data.Maintenance;
using NUnit.Framework;

namespace AmplaWeb.Data.Binding.Mapping.Modules
{
    [TestFixture]
    public class MaintenanceModuleMappingUnitTests : ModuleMappingTestFixture
    {
        public MaintenanceModuleMappingUnitTests() : base(MaintenanceViews.StandardView, () => new MaintenanceModuleMapping())
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
        public void SamplePeriod()
        {
            CheckField<DefaultValueFieldMapping>("SampleDateTime", "Sample Period", true, true);
        }

        [Test]
        public void SupportedOperations()
        {
            CheckAllowedOperations(
                ViewAllowedOperations.AddRecord,
                ViewAllowedOperations.ConfirmRecord,
                ViewAllowedOperations.DeleteRecord,
                ViewAllowedOperations.ModifyRecord,
                ViewAllowedOperations.UnconfirmRecord,
                ViewAllowedOperations.ViewRecord);
        }
    }
}