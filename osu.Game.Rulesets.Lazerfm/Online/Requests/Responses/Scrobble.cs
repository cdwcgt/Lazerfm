using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Game.Rulesets.Lazerfm.Helper;

namespace osu.Game.Rulesets.Lazerfm.Online.Requests.Responses
{
    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/Scrobble.cs
    // A Last.fm API model for an individual scrobble result
    public class Scrobble
    {
        // The corrected (or not) details associated with the artist
        [JsonProperty("artist")]
        public CorrectedStatus Artist { get; set; }

        // The corrected (or not) status of the scrobble
        [JsonProperty("ignoredMessage")]
        public IgnoredMessage IgnoredMessage { get; set; }

        // The corrected (or not) details associated with the album artist
        [JsonProperty("albumArtist")]
        public CorrectedStatus AlbumArtist { get; set; }

        // The corrected (or not) details associated with the album name
        [JsonProperty("album")]
        public CorrectedStatus Album { get; set; }

        // The corrected (or not) details associated with the track name
        [JsonProperty("track")]
        public CorrectedStatus Track { get; set; }
    }

    // https://github.com/lastfm/lastfm-windows-desktop/blob/master/DesktopScrobbler/LastFMApi/Models/CorrectedStatus.cs#L6
    public class CorrectedStatus
    {
        [JsonProperty("corrected")]
        // A number representing where a correct was made
        // 0 = there was no correction, 1 =  there was a correction
        public string Corrected { get; set; } = string.Empty;

        [JsonProperty("#text")]
        // The 'new' (or passed) text associated with the media item
        public string? CorrectedText { get; set; } = string.Empty;
    }

    // https://github.com/lastfm/lastfm-windows-desktop/blob/master/DesktopScrobbler/LastFMApi/Models/IgnoredMessage.cs
    public class IgnoredMessage
    {
        // The reason why the item was ignored (largely un-important unless it's because the API limit was exceeded)
        [JsonProperty("code")]
        public ReasonCodes.IgnoredReason Code { get; set; }
    }

    // https://github.com/lastfm/lastfm-windows-desktop/blob/master/DesktopScrobbler/LastFMApi/Models/Scrobbles.cs
    // A Last.fm API for a list of scrobbles, and their accepted status
    public class Scrobbles
    {
        // The overall acceptance of the scrobbles
        [JsonProperty("@attr")]
        public AcceptedResult AcceptedResult { get; set; }

        // The list of associated scrobble items
        [JsonProperty("scrobble")]
        [JsonConverter(typeof(SingleOrArrayConverter<Scrobble>))]
        public List<Scrobble> ScrobbleItems { get; set; }
    }

    // https://github.com/lastfm/lastfm-windows-desktop/blob/master/DesktopScrobbler/LastFMApi/Models/ScrobbleResponse.cs
    public class ScrobbleResponse
    {
        // The list of scrobble results
        [JsonProperty("scrobbles")]
        public Scrobbles Scrobbles { get; set; }
    }

    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/AcceptedStatus.cs#L6
    // Model representing the Accepted and Ignored state of a Scrobble
    public class AcceptedResult
    {
        // How many scrobble items were accpeted
        [JsonProperty("accepted")]
        public int Accepted { get; set; }

        // How many scrobble items were ignored
        [JsonProperty("ignored")]
        public int Ignored { get; set; }
    }
}
