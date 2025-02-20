using Newtonsoft.Json;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests.Responses
{
    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/PlayStatusResponse.cs#L6
    // A Last.fm scrobble response for when the API is notified that a track started playing
    public class PlayStatusResponse
    {
        // The response returned as a scrobble (despite it not really being one)
        [JsonProperty("nowPlaying")]
        public Scrobble NowPlaying { get; set; }
    }
}
