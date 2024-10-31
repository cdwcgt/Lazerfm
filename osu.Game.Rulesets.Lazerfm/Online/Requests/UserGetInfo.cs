using osu.Game.Rulesets.Lazerfm.Online.Requests.Responses;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public class UserGetInfo : LastfmRequest<UserInfoResponse>
    {
        public override string MethodName => "user.getInfo";

        public override bool RequiresSignature => false;

        public UserGetInfo(string username = "")
        {
            if (string.IsNullOrEmpty(username))
                return;

            Parameters.Add("user", username);
        }
    }
}
