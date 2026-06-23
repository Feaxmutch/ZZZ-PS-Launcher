namespace ZZZ_PS_Launcher
{
    public struct Profile
    {
        public Profile(string name, Patches patches, string serverCommit, ServerType serverType)
        {
            Name = name;
            Patches = patches;
            ServerCommit = serverCommit;
            ServerType = serverType;
        }

        public Profile(Profile profile) : this(profile.Name, profile.Patches, profile.ServerCommit, profile.ServerType) { }

        public Profile() : this(string.Empty, default, string.Empty, ServerType.Yoshunko) { }

        public string Name { get; }

        public ServerType ServerType { get; }

        public Patches Patches { get; }

        public string ServerCommit { get; }
    }
}
