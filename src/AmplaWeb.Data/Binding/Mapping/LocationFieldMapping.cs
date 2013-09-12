namespace AmplaWeb.Data.Binding.Mapping
{

    /// <summary>
    /// Location Field mapping used to store the location mapping
    /// </summary>
    public class LocationFieldMapping : ConstantFieldMapping
    {
        /// <summary>
        /// Creates a new Constant Field Mapping to store the location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public LocationFieldMapping(string name, string value) : base(name, value)
        {
        }
    }
}