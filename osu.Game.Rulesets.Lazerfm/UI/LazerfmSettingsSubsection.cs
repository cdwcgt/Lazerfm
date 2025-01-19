using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Lazerfm.Online;

namespace osu.Game.Rulesets.Lazerfm.UI
{
    public partial class LazerfmSettingsSubsection : RulesetSettingsSubsection
    {
        private OsuSpriteText userInfoText = null!;
        private SettingsButton loginButton = null!;
        protected override LocalisableString Header => "Lazerfm";

        [Resolved]
        private LastfmAPI lastfmAPI { get; set; } = null!;

        public LazerfmSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Child = userInfoText = new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    }
                },
                loginButton = new SettingsButton
                {
                    Text = "Login to lastfm",
                    Action = lastfmAPI.Login,
                }
            };

            lastfmAPI.IsLoggedIn.BindValueChanged(s =>
            {
                if (s.NewValue)
                {
                    userInfoText.Text = $"Logged in as {lastfmAPI.Username.Value}.";
                    userInfoText.Colour = colour.Green;
                    loginButton.Text = "Logout";
                    loginButton.Action = () => lastfmAPI.Logout();
                }
                else
                {
                    userInfoText.Text = "You have not login yet.";
                    userInfoText.Colour = colour.Red;
                    loginButton.Text = "Login to lastfm";
                    loginButton.Action = () => lastfmAPI.Login();
                }
            }, true);
        }
    }
}
