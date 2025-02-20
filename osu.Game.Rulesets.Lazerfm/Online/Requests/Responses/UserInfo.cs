using Newtonsoft.Json;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests.Responses
{
    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/UserInfo.cs
    public class UserInfo
    {
        // The name (username) of the user
        [JsonProperty("name")]
        public string Name { get; set; }

        // The user's profile Url
        [JsonProperty("url")]
        public string Url { get; set; }

        // The country of residence for the user
        [JsonProperty("country")]
        public string Country { get; set; }

        // How old the user is
        [JsonProperty("age")]
        public int Age { get; set; }

        // The gender of the user
        [JsonProperty("gender")]
        public string Gender { get; set; }

        // Whether the user is a subscriber or not
        [JsonProperty("subscriber")]
        public uint Subscriber { get; set; }

        // How many times the user has submitted 'Now playing' requests to the API
        [JsonProperty("playcount")]
        public long PlayCount { get; set; }

        // How many playlists the user has
        [JsonProperty("playlists")]
        public long Playlists { get; set; }
    }

    public class UserInfoResponse
    {
        public UserInfo User { get; set; }
    }
}
