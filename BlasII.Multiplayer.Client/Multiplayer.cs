using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Multiplayer.Client.Storages;
using BlasII.Multiplayer.Core;
using Il2CppTGK.Game;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    private readonly NetworkClient _client;

    public CompanionHandler CompanionHandler { get; }
    public NetworkHandler NetworkHandler { get; }
    public PlayerHandler PlayerHandler { get; }

    public AnimationStorage AnimationStorage { get; }

    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _client = new NetworkClient(new CoreSerializer());
        _client.OnClientConnected += TEMP_OnConnect;
        _client.OnClientDisconnected += TEMP_OnDisconnect;
        _client.OnPacketReceived += TEMP_OnReceive;

        CompanionHandler = new CompanionHandler();
        NetworkHandler = new NetworkHandler(_client);
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
        NetworkHandler.OnUpdate();
        PlayerHandler.OnUpdate();

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Equals))
        {
            NetworkHandler.Connect("localhost", 33002);
        }
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

    private void TEMP_OnConnect(string ip)
    {
        ModLog.Warn("Now connected");
    }

    private void TEMP_OnDisconnect(string ip)
    {
        ModLog.Warn("Now disconnected");
    }

    private void TEMP_OnReceive(BasePacket packet)
    {
        ModLog.Error("Received a packet");
    }
}
