using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Multiplayer.Client.Displays;
using BlasII.Multiplayer.Client.Nametags;
using BlasII.Multiplayer.Client.Storages;
using BlasII.Multiplayer.Core;
using Il2CppTGK.Game;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    private readonly NetworkClient _client;

    public CompanionHandler CompanionHandler { get; }
    public NametagHandler NametagHandler { get; }
    public NetworkHandler NetworkHandler { get; }
    public PlayerHandler PlayerHandler { get; }

    public StatusDisplay StatusDisplay { get; }

    public AnimationStorage AnimationStorage { get; }
    public IconStorage IconStorage { get; }

    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _client = new NetworkClient(new CoreSerializer());
        _client.OnClientConnected += TEMP_OnConnect;
        _client.OnClientDisconnected += TEMP_OnDisconnect;

        CompanionHandler = new CompanionHandler(_client);
        NametagHandler = new NametagHandler(_client);
        NetworkHandler = new NetworkHandler(_client);
        PlayerHandler = new PlayerHandler(_client);

        StatusDisplay = new StatusDisplay(_client);

        AnimationStorage = new AnimationStorage();
        IconStorage = new IconStorage(FileHandler);
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
        NametagHandler.OnUpdate();
        NetworkHandler.OnUpdate();
        PlayerHandler.OnUpdate();

        StatusDisplay.OnUpdate();

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Equals))
        {
            NetworkHandler.Connect(SERVER, PORT, new Models.RoomInfo(ROOM, PLAYER, TEAM));
        }
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "MainMenu")
            return;

        CompanionHandler.OnEnterScene();
        NametagHandler.OnEnterScene();
        PlayerHandler.OnEnterScene();
    }

    protected override void OnSceneUnloaded(string sceneName)
    {
        CompanionHandler.OnLeaveScene();
        NametagHandler.OnLeaveScene();
        PlayerHandler.OnLeaveScene();
    }

    // TODO: move these to the console popup + log
    private void TEMP_OnConnect(string ip)
    {
        ModLog.Info($"Connected to {ip}");
    }

    private void TEMP_OnDisconnect(string ip)
    {
        ModLog.Info($"Disconnected from {ip}");
    }

    private const string SERVER = "localhost";
    private const int PORT = 33002;
    private const string PLAYER = "Damocles";
    private const string ROOM = "TEST";
    private const int TEAM = 1;

    public static string PlayerName { get; } = PLAYER; // TEMP !!
}
