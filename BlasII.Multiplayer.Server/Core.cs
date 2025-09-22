using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using Basalt.Framework.Networking;
using Basalt.Framework.Networking.Server;
using BlasII.Multiplayer.Core;
using System;
using System.Threading;

namespace BlasII.Multiplayer.Server;

internal class Core
{
    static void Main(string[] args)
    {
        Logger.AddLoggers(new ConsoleLogger(TITLE), new FileLogger(Environment.CurrentDirectory));

        var server = new NetworkServer(new CoreSerializer());
        var room = new RoomManager(server);

        try
        {
            server.Start(PORT);
            Logger.Info($"Server started at {server.Ip}:{server.Port}");
        }
        catch (Exception ex)
        {
            Logger.Fatal($"Encountered an error when starting the server - {ex}");
            return;
        }

        try
        {
            ReadLoop(server);
        }
        catch (Exception ex)
        {
            Logger.Fatal($"Encountered an error when updating the server - {ex}");
            return;
        }
    }

    static void ReadLoop(NetworkServer server)
    {
        while (true)
        {
            Thread.Sleep(INTERVAL_MS);

            if (!server.IsActive)
                return;

            try
            {
                server.Receive();
            }
            catch (NetworkException ex)
            {
                Logger.Error($"Encountered an error when receiving data - {ex}");
            }

            server.Update();
        }
    }

    private const string TITLE = "Blasphemous 2 Multiplayer Server";
    private const int PORT = 33002;
    private const int INTERVAL_MS = 10;
}
