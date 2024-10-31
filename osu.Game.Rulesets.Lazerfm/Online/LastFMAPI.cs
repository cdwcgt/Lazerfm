using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Threading;
using osu.Game.Rulesets.Lazerfm.Configuration;
using osu.Game.Rulesets.Lazerfm.Online.Requests;
using Logger = osu.Framework.Logging.Logger;

namespace osu.Game.Rulesets.Lazerfm.Online
{
    [Cached]
    public partial class LastfmAPI : Component
    {
        private const string api_key = "c57b4a9fc29a13744c4ed8221b338792";
        private const string api_secret = "f7afcd3ac5f30a99814ddbe47d9fe602";

        [Resolved]
        private LazerfmRulesetConfigManager config { get; set; } = null!;

        [Resolved]
        private OsuGame? game { get; set; }

        private readonly Bindable<string> sessionKey = new Bindable<string>();

        private readonly Bindable<string> username = new Bindable<string>();

        public IBindable<string> Username => username;

        public Bindable<bool> IsLoggedIn { get; } = new Bindable<bool>();

        [BackgroundDependencyLoader]
        private void load()
        {
            config.BindWith(LazerfmSettings.LastFmUsername, username);
            config.BindWith(LazerfmSettings.LastFmSessionToken, sessionKey);

            sessionKey.BindValueChanged(s =>
            {
                if (string.IsNullOrEmpty(s.NewValue))
                {
                    IsLoggedIn.Value = false;
                    return;
                }

                IsLoggedIn.Value = true;
            }, true);

            checkLoginStatus();
            Scheduler.AddDelayed(checkLoginStatus, 500000, true);
        }

        public void Login()
        {
            var request = new AuthGetToken();

            request.Finished += () =>
            {
                game?.OpenUrlExternally($"http://www.last.fm/api/auth/?api_key={api_key}&token={request.ResponseObject.Token}");
                waitForSessionToken(request.ResponseObject.Token);
            };

            PerformAsync(request);
        }

        public void Logout()
        {
            username.Value = string.Empty;
            sessionKey.Value = string.Empty;
        }

        private void checkLoginStatus()
        {
            if (!IsLoggedIn.Value)
                return;

            var request = new UserGetInfo();

            request.Failed += e =>
            {
                if (request.Error != null)
                    Logout();
            };

            request.Finished += () =>
            {
                username.Value = request.ResponseObject.User.Name;
            };

            Perform(request, false);
        }

        public Task<T> PerformAsync<T>(LastfmRequest<T> request) =>
            Task.Factory.StartNew(() => Perform(request), TaskCreationOptions.LongRunning);

        private ScheduledDelegate? waitForSessionTokenDelegate;

        private void waitForSessionToken(string token)
        {
            waitForSessionTokenDelegate?.Cancel();

            waitForSessionTokenDelegate = Scheduler.AddDelayed(() =>
            {
                var request = new AuthGetSession(token);
                Perform(request, false);

                if (request.Completed && request.Error == null)
                {
                    username.Value = request.ResponseObject.SessionToken.Name;
                    sessionKey.Value = request.ResponseObject.SessionToken.Key;
                    waitForSessionTokenDelegate?.Cancel();
                }
            }, 1000, true);
        }

        public T Perform<T>(LastfmRequest<T> request, bool logWhenFail = true)
        {
            AddRequiredRequestParams(request.Parameters, request.MethodName, sessionKey.Value, request.RequiresSignature);

            try
            {
                request.Perform();
            }
            catch
            {
                if (request.Error != null && logWhenFail)
                {
                    Logger.Log($"Failed to send lastfm request, error code {request.Error.Error}, {request.Error.Message}", LoggingTarget.Network, LogLevel.Error);
                }
            }

            return request.ResponseObject;
        }

        public void AddRequiredRequestParams(Dictionary<string, string> requestParameters, string methodName, string sessionKey, bool requiresSignature = true)
        {
            // api key
            requestParameters.Add("api_key", api_key);

            // session key
            if (!string.IsNullOrEmpty(sessionKey))
            {
                requestParameters.Add("sk", sessionKey);
            }

            // api_sig if one is required by the method being called
            if (requiresSignature)
            {
                requestParameters.Add("api_sig", GetMethodSignature(requestParameters));
            }

            // Force responses to be in JSON
            requestParameters.Add("format", "json");
        }

        // Private method for building the signature required by the Last.fm API to validate a request
        public string GetMethodSignature(Dictionary<string, string>? methodParameters = null)
        {
            methodParameters ??= new Dictionary<string, string>();

            var builder = new StringBuilder();

            // Iterate all the parameters to be sent to the request
            foreach (var kv in methodParameters.OrderBy(kv => kv.Key, StringComparer.Ordinal))
            {
                // Append the key and value (with no separator)
                builder.Append(kv.Key);
                builder.Append(kv.Value);
            }

            // Add the API secret (required)
            builder.Append(api_secret);

            // Get the string, and MD5 hash it
            string hashedSignature = GetMd5Hash(builder.ToString());

            return hashedSignature;
        }

        public static string GetMd5Hash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);

            string hexString = Convert.ToHexString(hashBytes).ToLower();

            return hexString;
        }
    }
}
