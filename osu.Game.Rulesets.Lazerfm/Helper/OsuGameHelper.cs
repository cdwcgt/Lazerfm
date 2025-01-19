using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osu.Game.Configuration;
using osu.Game.Database;
using osu.Game.Rulesets.Lazerfm.Components;
using osu.Game.Rulesets.Lazerfm.Configuration;
using osu.Game.Rulesets.Lazerfm.Online;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Lazerfm.Helper
{
    public partial class OsuGameHelper : CompositeDrawable
    {
        /// <summary>
        /// 当前注入生效的游戏中 OsuGame 的 HashCode，-1则代表未曾注入过
        /// </summary>
        private static int currentSessionHash = -1;

        public static int GetRegisteredSessionHash()
        {
            return currentSessionHash;
        }

        public static DependencyContainer? GetGameDepManager(OsuGame? gameInstance)
        {
            return gameInstance?.Dependencies as DependencyContainer;
        }

        public static bool InjectDependencies(OsuGame gameInstance, Scheduler scheduler, RealmAccess realm, LazerfmRuleset ruleset)
        {
            int sessionHashCode = gameInstance.Toolbar.GetHashCode();

            if (currentSessionHash == sessionHashCode)
            {
                Logger.Log("Duplicate dependency inject call for current session, skipping...");
                return true;
            }

            currentSessionHash = sessionHashCode;

            var depMgr = GetGameDepManager(gameInstance);

            if (depMgr == null)
            {
                Logger.Log("DependencyContainer not found", level: LogLevel.Error);
                return false;
            }

            try
            {
                var scrobbler = new LazerScrobbler();
                var lastfmApi = new LastfmAPI();

                #region Remove Next Release

                var realmConfig = new LazerfmRulesetRealmConfigManager(new SettingsStore(realm), ruleset.RulesetInfo);
                var iniConfig = new LazerfmRulesetConfigManager(gameInstance.Dependencies.Get<Storage>());

                string? oldToken = realmConfig.Get<string>(LazerfmSettings.LastFmSessionToken);

                if (!string.IsNullOrEmpty(oldToken))
                {
                    iniConfig.SetValue(LazerfmSettings.LastFmSessionToken, oldToken);
                }

                realmConfig.SetValue<string>(LazerfmSettings.LastFmSessionToken, "");
                realmConfig.SetValue<string>(LazerfmSettings.LastFmUsername, "");

                depMgr.CacheAs(typeof(LazerfmRulesetRealmConfigManager), realmConfig);

                #endregion

                depMgr.CacheAs(typeof(LazerfmRulesetConfigManager), iniConfig);
                depMgr.Cache(scrobbler);
                depMgr.Cache(lastfmApi);

                scheduler.AddDelayed(() =>
                {
                    disableSentryLogger(gameInstance);

                    try
                    {
                        gameInstance.Add(scrobbler);
                        gameInstance.Add(lastfmApi);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, "cannot load components");
                    }
                }, 1);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Injection failed!, something may be wrong");
                return false;
            }

            return true;
        }

        private static void disableSentryLogger(OsuGame game)
        {
            var field = game.FindFieldInstance(typeof(SentryLogger));
            if (field == null) throw new NullDependencyException("没有找到SentryLogger");

            object? val = field.GetValue(game);
            if (val is not SentryLogger sentryLogger) throw new NullDependencyException($"获取的对象不是SentryLogger: {val}");

            sentryLogger.Dispose();
            Logger.Log("Disabled SentryLogger");
        }
    }
}
