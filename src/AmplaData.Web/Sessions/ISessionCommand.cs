namespace AmplaData.Web.Sessions
{
    /// <summary>
    ///     Interface for executing session commands
    /// </summary>
    public interface ISessionCommand
    {
        /// <summary>
        /// Execute the session command
        /// </summary>
        void Execute();
    }
}