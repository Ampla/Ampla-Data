using AmplaData.AmplaData2008;

namespace AmplaData.Database
{
    public interface IAmplaConfiguration
    {
        string[] GetLocations(string module);

        GetView GetViewForLocation(string module, string location);

        void SetDefaultView(string module, GetView defaultView);
    }
}