using System;
using System.Collections.Generic;
using System.Text;

namespace ZZZ_PS_Launcher_2
{
    public struct Profile
    {
        public Profile(string name, Patches patches, string serverCommit)
        {
            Name = name;
            Patches = patches;
            ServerCommit = serverCommit;
        }

        public Profile(Profile profile) : this(profile.Name, profile.Patches, profile.ServerCommit) { }

        public string Name { get; }

        public Patches Patches { get; }

        public string ServerCommit { get; }
    }
}
