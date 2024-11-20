// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Lazerfm.Beatmaps;
using osu.Game.Rulesets.Lazerfm.Helper;
using osu.Game.Rulesets.Lazerfm.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Lazerfm
{
    public partial class LazerfmRuleset : Ruleset
    {
        public override string Description => "Lazerfm";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod>? mods = null) =>
            new DrawableLazerfmRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new LazerfmBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new LazerfmDifficultyCalculator(RulesetInfo, beatmap);

        public override RulesetSettingsSubsection CreateSettings() => new LazerfmSettingsSubsection(this);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                default:
                    return Array.Empty<Mod>();
            }
        }

        public override string ShortName => "lazerfm";

        public override Drawable CreateIcon() => new LastfmIcon();

        public partial class LastfmIcon : CompositeDrawable
        {
            public LastfmIcon()
            {
                Size = new Vector2(20);
                InternalChildren =
                [
                    new SpriteIcon
                    {
                        Size = new Vector2(0.6f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = FontAwesome.Get(0xf202),
                        RelativeSizeAxes = Axes.Both,
                    }
                ];
            }

            [BackgroundDependencyLoader]
            private void load(OsuGame osuGame, RealmAccess realm)
            {
                // injected to the game.
                Scheduler.AddDelayed(() =>
                {
                    OsuGameHelper.InjectDependencies(osuGame, Scheduler, realm, new LazerfmRuleset());
                }, 50);
            }
        }

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
    }
}
