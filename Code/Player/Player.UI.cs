using SCE.HUD;

namespace SCE.Player;

public partial class Player
{
    /// <summary>
    /// The Screen Panel that will be used to render the HUD and other UI components.
    /// </summary>
    [Property]
    [Category("UI")]
    public ScreenPanel ScreenPanel { get; set; }

    MainHUD HUD;

    private void OnUIStart()
    {
        HUD = Components.Create<MainHUD>();
    }
}