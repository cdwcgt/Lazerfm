using System.Net.Http;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public class RemoveNowPlaying : LastfmRequest<object>
    {
        public override string MethodName => "track.removeNowPlaying";

        public RemoveNowPlaying()
        {
            Method = HttpMethod.Post;
        }
    }
}
