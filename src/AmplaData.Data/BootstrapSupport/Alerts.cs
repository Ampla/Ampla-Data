namespace AmplaData.Data.BootstrapSupport
{
    public static class Alerts
    {
        public const string Success = "success";
        public const string Attention = "attention";
        public const string Error = "error";
        public const string Information = "info";

        public static string[] All
        {
            get { 
                return new[] { Success, Attention, Information, Error };
            }
        }
    }
}