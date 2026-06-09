using Microsoft.Win32;
using System.Windows;

namespace ZZZ_PS_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Profile _currentProfile;

        public static string ProfilesPath => @"Software\ZZZ_PS_Launcher";

        public App()
        {
            _currentProfile = new(string.Empty, default, string.Empty);

            if (Registry.CurrentUser.OpenSubKey(ProfilesPath, true) == null)
            {
                Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("ZZZ_PS_Launcher");
            }
        }

        public static void SetProfile(Profile profile)
        {
            _currentProfile = profile;
        }

        public static Profile GetCurrentProfile()
        {
            return _currentProfile;
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
