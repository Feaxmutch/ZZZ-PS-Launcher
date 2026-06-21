namespace ZZZ_PS_Launcher
{
    public class CommitData
    {
        public CommitData(string name, string clientVersion, string value)
        {
            Name = name;
            ClientVersion = clientVersion;
            Value = value;
        }

        public string Name { get; }

        public string ClientVersion { get; }

        public string Value { get; }
    }
}
