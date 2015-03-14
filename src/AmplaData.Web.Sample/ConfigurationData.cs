namespace AmplaData.Web.Sample
{
    public class ConfigurationData
    {
        public bool ConnectToAmpla { get; private set; }
        public bool AddSampleData { get; private set; }

        public static ConfigurationData Default
        {
            get 
            { 
                return new ConfigurationData
                    {
                        ConnectToAmpla = true,
                        AddSampleData = false,
                    };
            }
        }

        public static ConfigurationData InMemory
        {
            get
            {
                return new ConfigurationData
                    {
                        ConnectToAmpla = false,
                        AddSampleData = true,
                    };
            }
        }
    }
}