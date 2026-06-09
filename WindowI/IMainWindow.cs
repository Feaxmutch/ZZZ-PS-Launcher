using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ_PS_Launcher_2.WindowI
{
    internal interface IMainWindow
    {
        event Action OpeningSettings;
        event Action LaunchingServer;
        event Action LaunchingClient;
    }
}
