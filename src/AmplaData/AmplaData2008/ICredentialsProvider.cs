namespace AmplaData.AmplaData2008
{
    /// <summary>
    /// Interface to provide credentials for the user
    /// </summary>
    public interface ICredentialsProvider
    {
        /// <summary>
        ///     Gets the credentials for the user
        /// </summary>
        /// <returns></returns>
        Credentials GetCredentials();
    }
}