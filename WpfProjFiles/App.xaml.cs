using Microsoft.Win32;
using System.Windows;

namespace ZZZ_PS_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string SelectedKeyName = "SelectedProfile";

        private static Profile _currentProfile;
        private static List<CommitData> _commits = new();

        public static IReadOnlyList<CommitData> Commits => _commits;

        public static string ProfilesPath => @"Software\ZZZ_PS_Launcher";

        public static CompatibilityAnalyzer CompatibilityAnalyzer { get; private set; }

        public App()
        {
            _currentProfile = new(string.Empty, default, string.Empty);

            if (Registry.CurrentUser.OpenSubKey(ProfilesPath, true) == null)
            {
                Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("ZZZ_PS_Launcher");
            }

            _commits.Add(new CommitData("Рекомендованый для 3.0.4 BETA", "1aff97a"));
            _commits.Add(new CommitData("Рекомендованый для 3.1.0 BETA", "31049ce"));
            _commits.Add(new CommitData("Рекомендованый для 2.8 PROD", "4ce69a6"));
            Dictionary<string, string> compatibilityList = new();
            compatibilityList.Add("CNBetaWin3.1.1", "master");
            compatibilityList.Add("CNBetaWin3.1.0", "31049ce");
            compatibilityList.Add("CNBetaWin3.0.4", "1aff97a");
            compatibilityList.Add("OSPRODWin2.8.0", "4ce69a6");
            compatibilityList.Add("OSPRODWin3.0.0", "prod");
            CompatibilityAnalyzer = new(compatibilityList);
            _currentProfile = RestoreSelectedProfile();
        }

        public static void SetProfile(Profile profile)
        {
            _currentProfile = profile;
        }

        public static Profile GetCurrentProfile()
        {
            return _currentProfile;
        }

        private static Profile RestoreSelectedProfile()
        {
            Profile[] profiles = GetAllProfiles();
            string selectedname = string.Empty;

            using (RegistryKey allProfilesKey = Registry.CurrentUser.OpenSubKey(ProfilesPath, true))
            {
                selectedname =(string)allProfilesKey.GetValue(SelectedKeyName, string.Empty);
            }


            if (profiles.Any(prof => prof.Name == selectedname))
            {
                return profiles.Where(prof => prof.Name == selectedname).First();
            }

            return new(string.Empty, default, string.Empty);
        }

        
        public static void SaveSelectedProfile()
        {
            using (RegistryKey allProfilesKey = Registry.CurrentUser.OpenSubKey(ProfilesPath, true))
            {
                allProfilesKey.SetValue(SelectedKeyName, _currentProfile.Name);
            }
        }

        public static void SaveProfile(Profile profile)
        {
            using (RegistryKey allProfilesKey = Registry.CurrentUser.OpenSubKey(ProfilesPath, true))
            {
                if (allProfilesKey == null)
                {
                    throw new InvalidOperationException($"Корневой путь {ProfilesPath} не найден в реестре.");
                }

                using (RegistryKey profileKey = allProfilesKey.CreateSubKey(profile.Name))
                {
                    if (profileKey != null)
                    {
                        profileKey.SetValue(ProfileKeyNames.ClientPath, profile.Patches.ClientPatch ?? "");
                        profileKey.SetValue(ProfileKeyNames.ServerPath, profile.Patches.ServerPatch ?? "");
                        profileKey.SetValue(ProfileKeyNames.HoyoSdkPath, profile.Patches.HoyoPatch ?? "");
                        profileKey.SetValue(ProfileKeyNames.KcpshimPath, profile.Patches.KcpshimPatch ?? "");
                        profileKey.SetValue(ProfileKeyNames.ServerCommit, profile.ServerCommit ?? "");
                    }
                }
            }
        }

        public static Profile[] GetAllProfiles()
        {
            using (RegistryKey profilesKey = Registry.CurrentUser.OpenSubKey(ProfilesPath, true))
            {
                string[] profileNames = profilesKey.GetSubKeyNames();
                Profile[] profiles = new Profile[profileNames.Length];

                for (int i = 0; i < profiles.Length; i++)
                {
                    using (RegistryKey profileKey = profilesKey.OpenSubKey(profileNames[i], true))
                    {
                        string clientPath = (string)profileKey.GetValue(ProfileKeyNames.ClientPath);
                        string serverPath = (string)profileKey.GetValue(ProfileKeyNames.ServerPath);
                        string hoyoPath = (string)profileKey.GetValue(ProfileKeyNames.HoyoSdkPath);
                        string kcpshimPath = (string)profileKey.GetValue(ProfileKeyNames.KcpshimPath);
                        string serverCommit = (string)profileKey.GetValue(ProfileKeyNames.ServerCommit);

                        Patches patches = new()
                        {
                            ClientPatch = clientPath,
                            ServerPatch = serverPath,
                            HoyoPatch = hoyoPath,
                            KcpshimPatch = kcpshimPath
                        };

                        profiles[i] = new Profile(profileNames[i], patches, serverCommit);
                    }
                }

                return profiles;
            }
        }
    }
}
