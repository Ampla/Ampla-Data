using AmplaData.Data.WebService;

namespace AmplaData.Data.AmplaSecurity2007
{
    public class SecurityWebServiceFactory : WebServiceFactory<ISecurityWebServiceClient>, ISecurityWebServiceClient
    {
        public CreateSessionResponse CreateSession(CreateSessionRequest request)
        {
            return Create().CreateSession(request);
        }

        public RenewSessionResponse RenewSession(RenewSessionRequest request)
        {
            return Create().RenewSession(request);
        }

        public ReleaseSessionResponse ReleaseSession(ReleaseSessionRequest request)
        {
            return Create().ReleaseSession(request);
        }
    }
}