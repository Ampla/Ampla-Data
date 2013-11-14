using System.Collections.Generic;

namespace AmplaData.Records
{
    public class ModelVersions
    {
        public ModelVersions(AmplaRecord amplaRecord)
        {
            Id = amplaRecord.Id;
            Location = amplaRecord.Location;
            Module = amplaRecord.Module;
            Versions = new List<ModelVersion>();
        }

        /// <summary>
        /// The Record Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// The Ampla Module
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        ///     The name of the Model
        /// </summary>
        public string ModelName { get; set; }

        public IList<ModelVersion> Versions { get; private set; }
    }
}