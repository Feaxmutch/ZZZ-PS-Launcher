using System.IO;
using System.Windows;

namespace ZZZ_PS_Launcher
{
    public class CompatibilityAnalyzer
    {
        private const string ClientVersionFile = "version_info";

        private List<CommitData> _compatibilityList = new();

        public CompatibilityAnalyzer(List<CommitData> compatibilityList)
        {
            _compatibilityList = compatibilityList;
        }

        public CheckVersionResult IsCommitVersionCorrect(Profile profile)
        {
            string clientDirectory = Path.GetDirectoryName(profile.Patches.ClientPatch) ?? string.Empty;
            string versionPath = Path.Combine(clientDirectory, ClientVersionFile);

            if (File.Exists(versionPath) == false)
            {
                return CheckVersionResult.NotFound;
            }

            string info = File.ReadAllLines(versionPath).First();

            if (_compatibilityList.Any(commit => commit.ClientVersion == info) == false)
            {
                return CheckVersionResult.Unknown;
            }

            if (_compatibilityList.Any(commit => commit.Value == profile.ServerCommit) == false)
            {
                return CheckVersionResult.Uncorrect;
            }

            return CheckVersionResult.Correct;
        }

        public bool AskForContinue(CheckVersionResult checkResult)
        {
            string title = "Предупреждение о совместимости";
            string message = string.Empty;

            switch (checkResult)
            {
                case CheckVersionResult.Uncorrect:
                    message = "Версия клиента не соответствует выбранному коммиту. Из-за этого клиент может не обнаружить сервер. Вы всё ещё хотите продолжить?";
                    break;
                case CheckVersionResult.Unknown:
                    message = "Не удалось проверить совместимость, так как приложение не знает о данной версии клиента. Рекомендуестся собрать последнюю версию приложения. Вы всё ещё хотите продолжить?";
                    break;
                case CheckVersionResult.NotFound:
                    message = "Не удалось проверить совместимость, так как в папке клиента не найден файл version_info. Приложение не гарантирует успешное подключение к серверу. Вы всё ещё хотите продолжить?";
                    break;
            }

            if (checkResult != CheckVersionResult.Correct)
            {
                MessageBoxResult messageResult = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageResult == MessageBoxResult.No)
                {
                    return false;
                }
            }

            return true;
        }
    }
}