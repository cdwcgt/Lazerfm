// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Lazerfm.Objects;
using osu.Game.Rulesets.Lazerfm.Objects.Drawables;
using osu.Game.Rulesets.Lazerfm.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Lazerfm.UI
{
    [Cached]
    public partial class DrawableLazerfmRuleset : DrawableRuleset<LazerfmHitObject>
    {
        public DrawableLazerfmRuleset(LazerfmRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod>? mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override Playfield CreatePlayfield() => new LazerfmPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new LazerfmFramedReplayInputHandler(replay);

        public override DrawableHitObject<LazerfmHitObject> CreateDrawableRepresentation(LazerfmHitObject h) => new DrawableLazerfmHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new LazerfmInputManager(Ruleset?.RulesetInfo);
    }
}
