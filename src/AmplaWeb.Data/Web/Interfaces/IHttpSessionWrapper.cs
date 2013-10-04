namespace AmplaWeb.Data.Web.Interfaces
{
    public interface IHttpSessionWrapper
    {
        void SetValue(string key, object value);
        object GetValue(string key);
    }
}