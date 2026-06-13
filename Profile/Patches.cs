namespace ZZZ_PS_Launcher
{
    public struct Patches : IPatches
    {
        public string ServerPatch { get; set; }
        public string HoyoPatch { get; set; }
        public string KcpshimPatch { get; set; }
        public string ClientPatch { get; set; }
    }

    public interface IPatches
    {
        string ServerPatch { get; }
        string HoyoPatch { get; }
        string KcpshimPatch { get; }
        string ClientPatch { get; }
    }
}
