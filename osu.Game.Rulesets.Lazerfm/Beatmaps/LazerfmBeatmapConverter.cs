﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Lazerfm.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Lazerfm.Beatmaps
{
    public class LazerfmBeatmapConverter : BeatmapConverter<LazerfmHitObject>
    {
        public LazerfmBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        // todo: Check for conversion types that should be supported (ie. Beatmap.HitObjects.Any(h => h is IHasXPosition))
        // https://github.com/ppy/osu/tree/master/osu.Game/Rulesets/Objects/Types
        public override bool CanConvert() => false;

        protected override IEnumerable<LazerfmHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            yield return new LazerfmHitObject
            {
                Samples = original.Samples,
                StartTime = original.StartTime,
            };
        }
    }
}
