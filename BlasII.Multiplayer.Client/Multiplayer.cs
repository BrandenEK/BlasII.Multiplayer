using BlasII.ModdingAPI;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnInitialize()
    {
        // Perform initialization here
    }
}
