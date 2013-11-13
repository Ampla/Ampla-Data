namespace AmplaData.Data.AmplaData2008
{
    public class CredentialsProvider : ICredentialsProvider
    {
        private readonly string session;
        private readonly string userName;
        private readonly string password;

        private CredentialsProvider(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        private CredentialsProvider(string session)
        {
            this.session = session;
        }

        public Credentials GetCredentials()
        {
            if (session == null)
            {
                return new Credentials {Username = userName, Password = password};
            }
            return new Credentials {Session = session};
        }

        public static CredentialsProvider ForSession(string session)
        {
            return new CredentialsProvider(session);
        }

        public static CredentialsProvider ForUsernameAndPassword(string userName, string password)
        {
            return new CredentialsProvider(userName, password);
        }
    }
}