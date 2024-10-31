using System.Net.Http;
using osu.Game.Rulesets.Lazerfm.Online.Requests.Responses;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public class TrackUpdateNowPlaying : LastfmRequest<PlayStatusResponse>
    {
        public override string MethodName => "track.updateNowPlaying";

        public TrackUpdateNowPlaying(string artist, string title)
        {
            Method = HttpMethod.Post;
            Parameters.Add("artist", artist);
            Parameters.Add("track", title);
        }
    }
}
