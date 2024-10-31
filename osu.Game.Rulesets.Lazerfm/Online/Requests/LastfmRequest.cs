using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using osu.Framework.IO.Network;
using osu.Game.Online.API;
using osu.Game.Rulesets.Lazerfm.Online.Requests.Responses;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests
{
    public abstract class LastfmRequest<T> : OsuJsonWebRequest<T>
    {
        public abstract string MethodName { get; }
        protected string BaseUrl => "https://ws.audioscrobbler.com/2.0/";

        public virtual bool RequiresSignature => true;

        public readonly Dictionary<string, string> Parameters = new();

        public ResponseError? Error;

        protected LastfmRequest()
        {
            Url = $"{BaseUrl}";
            Parameters.Add("method", MethodName);

            Failed += _ =>
            {
                string? response = GetResponseString();

                if (!string.IsNullOrEmpty(response))
                {
                    Error = JsonConvert.DeserializeObject<ResponseError>(response);
                }
            };
        }

        protected override void PrePerform()
        {
            base.PrePerform();

            if (Method == HttpMethod.Get)
            {
                foreach (var param in Parameters)
                {
                    AddParameter(param.Key, param.Value, RequestParameterType.Query);
                }

                AddParameter("format", "json");
                return;
            }

            var formData = new FormUrlEncodedContent(Parameters);
            AddRaw(formData.ReadAsStream());
        }
    }
}
