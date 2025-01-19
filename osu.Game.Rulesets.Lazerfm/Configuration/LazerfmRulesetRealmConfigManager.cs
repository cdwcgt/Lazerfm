using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Lazerfm.Configuration
{
    public class LazerfmRulesetRealmConfigManager : RulesetConfigManager<LazerfmSettings>
    {
        public LazerfmRulesetRealmConfigManager(SettingsStore? settings, RulesetInfo ruleset, int? variant = null)
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
}
