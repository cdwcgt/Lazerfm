// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Lazerfm.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Lazerfm.Replays
{
    public class LazerfmAutoGenerator : AutoGenerator<LazerfmReplayFrame>
    {
        public new Beatmap<LazerfmHitObject> Beatmap => (Beatmap<LazerfmHitObject>)base.Beatmap;

        public LazerfmAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new LazerfmReplayFrame());

            foreach (LazerfmHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new LazerfmReplayFrame
                {
                    Time = hitObject.StartTime,
                    Position = hitObject.Position,
                    // todo: add required inputs and extra frames.
                });
            }
        }
    }
}
