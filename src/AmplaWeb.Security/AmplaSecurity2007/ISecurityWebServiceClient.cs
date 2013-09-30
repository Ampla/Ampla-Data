namespace AmplaWeb.Security.AmplaSecurity2007
{
    public interface ISecurityWebServiceClient
    {
        CreateSessionResponse CreateSession(CreateSessionRequest request);
        RenewSessionResponse RenewSession(RenewSessionRequest request);
        ReleaseSessionResponse ReleaseSession(ReleaseSessionRequest request);
    }
}