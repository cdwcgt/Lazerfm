﻿using System;
using Newtonsoft.Json;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Lazerfm.Components
{
    // https://github.com/lastfm/lastfm-windows-desktop/blob/52072e40c0f1198bdafccd4aea10841033a5650a/DesktopScrobbler/LastFMApi/Models/MediaItem.cs
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
            string trackName = workingBeatmap.BeatmapInfo.Metadata.TitleUnicode;
            string artistName = workingBeatmap.BeatmapInfo.Metadata.ArtistUnicode;

            if (string.IsNullOrEmpty(trackName))
            {
                trackName = workingBeatmap.BeatmapInfo.Metadata.Title;
            }

            if (string.IsNullOrEmpty(artistName))
            {
                artistName = workingBeatmap.BeatmapInfo.Metadata.Artist;
            }

            return new MediaItem
            {
                TrackName = trackName,
                ArtistName = artistName,
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
