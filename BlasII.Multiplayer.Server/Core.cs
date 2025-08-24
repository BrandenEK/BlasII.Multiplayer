using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using System;
using System.Threading;

namespace BlasII.Multiplayer.Server;

internal class Core
{
    static void Main(string[] args)
    {
        Logger.AddLoggers(new ConsoleLogger(TITLE), new FileLogger(Environment.CurrentDirectory));

        Logger.Warn("This is a test warning");

        while (true)
        {
            Thread.Sleep(100);
        }
    }

    private const string TITLE = "Blasphemous 2 Multiplayer Server";
}
