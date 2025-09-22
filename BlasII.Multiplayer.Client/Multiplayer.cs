using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Client;
using Basalt.Framework.Networking.Serializers;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Multiplayer.Client.Storages;
using Il2CppTGK.Game;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    public NetworkClient NetworkClient { get; }

    public CompanionHandler CompanionHandler { get; }
    public PlayerHandler PlayerHandler { get; }

    public AnimationStorage AnimationStorage { get; }

    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        NetworkClient = new NetworkClient(new ClassicSerializer());
        NetworkClient.OnClientConnected += TEMP_OnConnect;
        NetworkClient.OnClientDisconnected += TEMP_OnDisconnect;
        NetworkClient.OnPacketReceived += TEMP_OnReceive;

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

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Equals))
        {
            // Connect
            NetworkConnect();
        }

        NetworkUpdate();
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

    private void NetworkConnect()
    {
        ModLog.Info($"Trying to connect to localhost:33002");

        try
        {
            NetworkClient.Connect("localhost", 33002);
            ModLog.Info($"Client connected to localhost:33002");
        }
        catch (System.Exception ex)
        {
            ModLog.Error($"Encountered an error when attempting to connect - {ex}");
            return;
        }
    }

    private void NetworkUpdate()
    {
        if (!NetworkClient.IsActive)
            return;

        try
        {
            NetworkClient.Receive();
        }
        catch (NetworkException ex)
        {
            ModLog.Error($"Encountered an error when receiving data - {ex}");
        }

        NetworkClient.Update();
    }
}
