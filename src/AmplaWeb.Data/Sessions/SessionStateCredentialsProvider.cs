using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Sessions
{
    /// <summary>
    ///     Credentials Provider that extracts the session from HttpSession or the backup credentials provider
    /// </summary>
    public class SessionStateCredentialsProvider : ICredentialsProvider
    {
        private readonly IAmplaSessionStorage amplaSessionStorage;

        public SessionStateCredentialsProvider(IAmplaSessionStorage amplaSessionStorage)
        {
            this.amplaSessionStorage = amplaSessionStorage;
        }

        /// <summary>
        /// Gets the credentials for the user
        /// </summary>
        /// <returns></returns>
        public Credentials GetCredentials()
        {
            if (amplaSessionStorage.Enabled)
            {
                string session = amplaSessionStorage.GetAmplaSession();
                if (!string.IsNullOrEmpty(session))
                {
                    return CredentialsProvider.ForSession(amplaSessionStorage.GetAmplaSession()).GetCredentials();
                }
            }
            return null;
        }
    }
}