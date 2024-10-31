using osu.Game.Rulesets.Lazerfm.Online.Requests.Responses;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public class AuthGetToken : LastfmRequest<AuthToken>
    {
        public override string MethodName => "auth.getToken";
    }
}
