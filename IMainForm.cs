using System;
using System.Collections.Generic;
using System.Text;

namespace ZZZ_PS_Launcher
{
    internal interface IMainForm
    {
        event Action OpeningSettings;
        event Action LaunchingServer;
        event Action LaunchingClient;
    }
}
