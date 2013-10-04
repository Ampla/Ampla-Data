
using AmplaWeb.Data.AmplaData2008;

namespace AmplaWeb.Data.Session
{
    /// <summary>
    ///     Credentials Provider that extracts the session from HttpSession
    /// </summary>
    public class SessionStateCredentialsProvider : ICredentialsProvider
    {
        private readonly IAmplaSessionStorage amplaSessionStorage;

        public SessionStateCredentialsProvider(IAmplaSessionStorage amplaSessionStorage)
        {
            this.amplaSessionStorage = amplaSessionStorage;
        }

        public Credentials GetCredentials()
        {
            return CredentialsProvider.ForSession(amplaSessionStorage.GetAmplaSession()).GetCredentials();
        }
    }
}