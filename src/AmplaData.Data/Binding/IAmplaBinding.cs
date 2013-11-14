
namespace AmplaData.Binding
{
    /// <summary>
    ///     Interface for Binding Models to Ampla Webservice operations
    /// </summary>
    public interface IAmplaBinding
    {
        bool Bind();

        bool Validate();
    }
}