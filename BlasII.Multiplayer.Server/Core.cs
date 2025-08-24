using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using Basalt.Framework.Networking.Serializers;
using Basalt.Framework.Networking.Server;
using System;
using System.Threading;

namespace BlasII.Multiplayer.Server;

internal class Core
{
    static void Main(string[] args)
    {
        Logger.AddLoggers(new ConsoleLogger(TITLE), new FileLogger(Environment.CurrentDirectory));

        var room = new RoomManager();

        var server = new NetworkServer(new ClassicSerializer());
        server.OnClientConnected += room.OnClientConnected;
        server.OnClientDisconnected += room.OnClientDisconnected;
        server.OnPacketReceived += room.OnPacketReceived;

        try
        {
            server.Start(33002);
            Logger.Info($"Server started at {server.Ip}:{server.Port}");
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex);
            return;
        }

        while (true)
        {
            Thread.Sleep(100);

            if (!server.IsActive)
                return;

            server.Receive();
            server.Update();
        }
    }

    private const string TITLE = "Blasphemous 2 Multiplayer Server";
}
