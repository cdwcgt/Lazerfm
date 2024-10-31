using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Lazerfm.Configuration
{
    public class LazerfmRulesetConfigManager : RulesetConfigManager<LazerfmSettings>
    {
        public LazerfmRulesetConfigManager(SettingsStore? settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetDefault(LazerfmSettings.LastFmUsername, string.Empty);
            SetDefault(LazerfmSettings.LastFmSessionToken, string.Empty);
        }
    }

    public enum LazerfmSettings
    {
        LastFmUsername,
        LastFmSessionToken,
    }
}
