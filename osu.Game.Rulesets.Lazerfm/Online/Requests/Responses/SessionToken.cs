using Newtonsoft.Json;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests.Responses
{
    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/Session.cs
    public class Session
    {
        // The session token
        [JsonProperty("session")]
        public SessionToken SessionToken { get; set; }
    }

    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/SessionToken.cs
    // A Last.fm API model for a session token
    public class SessionToken
    {
        // The name of the user (username)
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        // The session key
        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;

        // Whether the user is a subscriber
        [JsonProperty("subscriber")]
        public string Subscriber { get; set; } = string.Empty;
    }
}
