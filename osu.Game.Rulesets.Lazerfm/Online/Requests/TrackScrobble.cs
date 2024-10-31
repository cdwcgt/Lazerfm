using System.Collections.Generic;
using System.Net.Http;
using osu.Game.Rulesets.Lazerfm.Components;
using osu.Game.Rulesets.Lazerfm.Online.Requests.Responses;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public class TrackScrobble : LastfmRequest<ScrobbleResponse>
    {
        public override string MethodName => "track.scrobble";

        public TrackScrobble(IEnumerable<MediaItem> items)
        {
            int itemCount = 0;

            foreach (MediaItem item in items)
            {
                Parameters.Add($"track[{itemCount}]", item.TrackName);
                Parameters.Add($"artist[{itemCount}]", item.ArtistName);
                Parameters.Add($"duration[{itemCount}]", item.TrackLength.ToString());
                Parameters.Add($"timestamp[{itemCount}]", item.StartedPlaying.ToUnixTimeSeconds().ToString());
                itemCount++;
            }

            Method = HttpMethod.Post;
        }
    }
}
