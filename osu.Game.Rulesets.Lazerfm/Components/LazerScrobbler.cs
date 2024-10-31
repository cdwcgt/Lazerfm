using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Input;
using osu.Game.Overlays;
using osu.Game.Rulesets.Lazerfm.Online;
using osu.Game.Rulesets.Lazerfm.Online.Requests;

namespace osu.Game.Rulesets.Lazerfm.Components
{
    public partial class LazerScrobbler : Component
    {
        [Resolved]
        private MusicController music { get; set; } = null!;

        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; } = null!;

        [Resolved]
        private LastfmAPI lastfm { get; set; } = null!;

        [Resolved]
        private IdleTracker idleTracker { get; set; } = null!;

        private readonly List<MediaItem> queuedItems = new List<MediaItem>();

        private readonly BindableBool isPlaying = new BindableBool();

        private bool isInNowPlayingStatus;

        private static ScrobbleStatus status;
        private static double timeElapsed;

        private MediaItem? lastMediaItem;

        [BackgroundDependencyLoader]
        private void load()
        {
            Scheduler.AddDelayed(sendScrobble, 300000, true);
        }

        protected override void Update()
        {
            base.Update();

            if (idleTracker.IsIdle.Value)
            {
                removeNowPlaying();
                return;
            }

            var currentMediaItem = MediaItem.FromWorkingBeatmap(beatmap.Value);

            if (music.IsPlaying && (lastMediaItem?.TrackName != currentMediaItem.TrackName && lastMediaItem?.ArtistName != currentMediaItem.ArtistName))
            {
                removeNowPlaying();
                lastMediaItem = currentMediaItem;
                status = ScrobbleStatus.Pending;
                timeElapsed = 0;
            }

            if (music.IsPlaying && lastMediaItem != null)
            {
                timeElapsed += Time.Elapsed;

                switch (status)
                {
                    case ScrobbleStatus.Pending:
                        if (timeElapsed > 3000)
                        {
                            status = ScrobbleStatus.NowPlaying;
                            sendNowPlaying(lastMediaItem);
                        }

                        break;

                    case ScrobbleStatus.NowPlaying:
                        if (timeElapsed > calculateScrobbleWaitingTime(music.CurrentTrack.Length))
                        {
                            status = ScrobbleStatus.Scrobbled;
                            queuedItems.Add(lastMediaItem);
                        }

                        break;

                    case ScrobbleStatus.Scrobbled:
                        if (timeElapsed > music.CurrentTrack.Length)
                        {
                            lastMediaItem = currentMediaItem;
                            status = ScrobbleStatus.Pending;
                            timeElapsed = 0;
                        }

                        break;
                }
            }
        }

        private void sendScrobble()
        {
            if (!lastfm.IsLoggedIn.Value)
                return;

            var request = new TrackScrobble(queuedItems);
            lastfm.PerformAsync(request).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    queuedItems.Clear();
                }
            });
        }

        private void sendNowPlaying(MediaItem? mediaItem)
        {
            if (mediaItem == null)
            {
                removeNowPlaying();
                return;
            }

            if (!lastfm.IsLoggedIn.Value)
                return;

            string title = mediaItem.TrackName;
            string artist = mediaItem.ArtistName;

            var request = new TrackUpdateNowPlaying(artist, title);
            lastfm.PerformAsync(request);
            isInNowPlayingStatus = true;
        }

        private void removeNowPlaying()
        {
            if (!lastfm.IsLoggedIn.Value)
                return;

            if (!isInNowPlayingStatus)
                return;

            var request = new RemoveNowPlaying();
            lastfm.PerformAsync(request);
            isInNowPlayingStatus = false;
        }

        private static bool checkTrackLength(ITrack track) => track.Length > 30000;

        // the track has been played for at least half its duration, or for 4 minutes (whichever occurs earlier.)
        private static double calculateScrobbleWaitingTime(double length) => length >= 800000 ? 400000 : length / 2;

        private enum ScrobbleStatus
        {
            Pending,
            NowPlaying,
            Scrobbled
        }
    }
}
