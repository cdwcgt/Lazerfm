using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Lazerfm.Configuration
{
    public class LazerfmRulesetConfigManager : IniConfigManager<LazerfmSettings>
    {
        protected override string Filename => "lastFm.ini";

        public LazerfmRulesetConfigManager(Storage storage)
            : base(storage)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetDefault(LazerfmSettings.LastFmSessionToken, string.Empty);
        }
    }

    public enum LazerfmSettings
    {
        LastFmSessionToken,

        #region Remove Next Release

        LastFmUsername

        #endregion
    }
}
