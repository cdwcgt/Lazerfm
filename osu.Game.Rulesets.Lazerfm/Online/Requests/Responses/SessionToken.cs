using Newtonsoft.Json;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests.Responses
{
    public class Session
    {
        // The session token
        [JsonProperty("session")]
        public SessionToken SessionToken { get; set; }
    }

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
