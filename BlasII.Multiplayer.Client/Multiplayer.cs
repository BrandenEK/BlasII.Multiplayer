using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Multiplayer.Client.Storages;
using Il2CppTGK.Game;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    public CompanionHandler CompanionHandler { get; }
    public PlayerHandler PlayerHandler { get; }

    public AnimationStorage AnimationStorage { get; }

    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        CompanionHandler = new CompanionHandler();
        PlayerHandler = new PlayerHandler();

        AnimationStorage = new AnimationStorage();
    }

    protected override void OnInitialize()
    {
        AnimationStorage.Initialize();
    }

    protected override void OnLateUpdate()
    {
        if (!SceneHelper.GameSceneLoaded || CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        CompanionHandler.OnUpdate();
        PlayerHandler.OnUpdate();
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "MainMenu")
            return;

        CompanionHandler.OnEnterScene();
        PlayerHandler.OnEnterScene();
    }

    protected override void OnSceneUnloaded(string sceneName)
    {
        CompanionHandler.OnLeaveScene();
        PlayerHandler.OnLeaveScene();
    }
}
