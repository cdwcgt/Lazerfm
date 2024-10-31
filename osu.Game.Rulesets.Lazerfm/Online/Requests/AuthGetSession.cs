using osu.Game.Rulesets.Lazerfm.Online.Requests.Responses;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public class AuthGetSession : LastfmRequest<Session>
    {
        public override string MethodName => "auth.getSession";

        public AuthGetSession(string authToken)
        {
            Parameters.Add("token", authToken);
        }
    }
}
