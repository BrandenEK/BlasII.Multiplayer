using MelonLoader;

namespace BlasII.Multiplayer.Client;

internal class Main : MelonMod
{
    public static Multiplayer Multiplayer { get; private set; }

    public override void OnLateInitializeMelon()
    {
        Multiplayer = new Multiplayer();
    }
}