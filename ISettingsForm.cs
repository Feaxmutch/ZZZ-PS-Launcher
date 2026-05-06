
namespace ZZZ_PS_Launcher
{
    internal interface ISettingsForm
    {
        event Action<App> ClickedSelect;
        event Action ClickedSave;
        event Action Showing;
        event Action Hiding;

        IPatches Patches { get; }

        void UpdateTextBoxes();
        void ApplyFromTextBoxes();
        void Close();
    }
}
