using System;
using Newtonsoft.Json;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Lazerfm.Components
{
    // A Last.fm API model for a media item
    public class MediaItem
    {
        // The name of the track
        [JsonProperty("track")]
        public string TrackName { get; set; }

        // The duration of the track (in seconds)
        [JsonProperty("duration")]
        public double TrackLength { get; set; }

        // The name of the track artist
        [JsonProperty("artist")]
        public string ArtistName { get; set; }

        // The time the track started playing (not passed to the API) but used to convert this
        // value by the appropriate API function into a Unix timestamp
        public DateTimeOffset StartedPlaying { get; set; }

        public static MediaItem FromWorkingBeatmap(IWorkingBeatmap workingBeatmap)
        {
            return new MediaItem
            {
                TrackName = workingBeatmap.BeatmapInfo.Metadata.TitleUnicode,
                ArtistName = workingBeatmap.BeatmapInfo.Metadata.ArtistUnicode,
                TrackLength = workingBeatmap.Track.Length / 1000,
                StartedPlaying = DateTimeOffset.UtcNow
            };
        }

        public static MediaItem FromWorkingBeatmap(IWorkingBeatmap workingBeatmap, DateTimeOffset startedPlayingTime)
        {
            var item = FromWorkingBeatmap(workingBeatmap);
            item.StartedPlaying = startedPlayingTime;
            return item;
        }
    }
}
