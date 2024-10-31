using Newtonsoft.Json;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests.Responses
{
    public class ResponseError
    {
        [JsonProperty("error")]
        public ReasonCodes.ErrorCode Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}
