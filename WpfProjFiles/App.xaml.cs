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
        private static List<CommitData> _yoshunkoCommits = new();
        private static List<CommitData> _remielleCommits = new();

        public static IReadOnlyList<CommitData> YoshunkoCommits => _yoshunkoCommits;

        public static IReadOnlyList<CommitData> RemielleCommits => _remielleCommits;

        public static string ProfilesPath => @"Software\ZZZ_PS_Launcher";

        public static CompatibilityAnalyzer YoshunkoCompatibility { get; private set; }

        public static CompatibilityAnalyzer RemielleCompatibility { get; private set; }

        public App()
        {
            _currentProfile = new();

            if (Registry.CurrentUser.OpenSubKey(ProfilesPath, true) == null)
            {
                Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("ZZZ_PS_Launcher");
            }

            _yoshunkoCommits.Add(new CommitData("Самый последний BETA", "CNBetaWin3.1.2", "master"));
            _yoshunkoCommits.Add(new CommitData("Самый последний PROD", "OSPRODWin3.0.0", "prod"));
            _yoshunkoCommits.Add(new CommitData("Рекомендованый для 3.1.1 BETA", "CNBetaWin3.1.1", "009742d"));
            _yoshunkoCommits.Add(new CommitData("Рекомендованый для 3.1.0 BETA", "CNBetaWin3.1.0", "31049ce"));
            _yoshunkoCommits.Add(new CommitData("Рекомендованый для 3.0.4 BETA", "CNBetaWin3.0.4", "1aff97a"));
            _yoshunkoCommits.Add(new CommitData("Рекомендованый для 2.8 PROD", "OSPRODWin2.8.0", "4ce69a6"));
            YoshunkoCompatibility = new(_yoshunkoCommits);

            _remielleCommits.Add(new CommitData("Самый последний BETA", "CNBetaWin3.1.3", "master"));
            _remielleCommits.Add(new CommitData("Рекомендованый для 3.1.2 BETA", "CNBetaWin3.1.2", "8960978"));
            RemielleCompatibility = new(_remielleCommits);
            _currentProfile = RestoreSelectedProfile();
        }

        public static void SetCurrentProfile(Profile profile)
        {
            _currentProfile = profile;
            SaveSelectedProfile();
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
                selectedname = (string)allProfilesKey.GetValue(SelectedKeyName, string.Empty);
            }


            if (profiles.Any(prof => prof.Name == selectedname))
            {
                return profiles.Where(prof => prof.Name == selectedname).First();
            }

            return new Profile();
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
                        profileKey.SetValue(ProfileKeyNames.ServerType, profile.ServerType.ToString() ?? "");
                    }
                }
            }
        }

        public static void RemoveProfile(Profile profile)
        {
            using (RegistryKey allProfilesKey = Registry.CurrentUser.OpenSubKey(ProfilesPath, true))
            {
                if (allProfilesKey == null)
                {
                    throw new InvalidOperationException($"Корневой путь {ProfilesPath} не найден в реестре.");
                }

                if (allProfilesKey.GetSubKeyNames().Contains(profile.Name))
                {
                    if (GetCurrentProfile().Name == profile.Name)
                    {
                        SetCurrentProfile(new Profile());
                        SaveSelectedProfile();
                    }

                    allProfilesKey.DeleteSubKey(profile.Name);
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
                    using (RegistryKey? profileKey = profilesKey.OpenSubKey(profileNames[i], true))
                    {
                        string clientPath = (string)profileKey.GetValue(ProfileKeyNames.ClientPath);
                        string serverPath = (string)profileKey.GetValue(ProfileKeyNames.ServerPath);
                        string hoyoPath = (string)profileKey.GetValue(ProfileKeyNames.HoyoSdkPath);
                        string kcpshimPath = (string)profileKey.GetValue(ProfileKeyNames.KcpshimPath);
                        string serverCommit = (string)profileKey.GetValue(ProfileKeyNames.ServerCommit);
                        ServerType serverType = Enum.Parse<ServerType>(profileKey.GetValue(ProfileKeyNames.ServerType).ToString());

                        Patches patches = new()
                        {
                            ClientPatch = clientPath,
                            ServerPatch = serverPath,
                            HoyoPatch = hoyoPath,
                            KcpshimPatch = kcpshimPath
                        };

                        profiles[i] = new Profile(profileNames[i], patches, serverCommit, serverType);
                    }
                }

                return profiles;
            }
        }
    }
}
