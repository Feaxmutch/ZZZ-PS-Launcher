namespace ZZZ_PS_Launcher.WindowI
{
    internal interface ICreateProfileWindow
    {
        event Action<ProfileSettingName> ClickedSelect;
        event Action ClickedSave;
        event Action Showing;
        event Action Hiding;

        IPatches Patches { get; }

        void SetTextBox(ProfileSettingName name, string value);
        string GetTextBox(ProfileSettingName name);
        void ApplyFromTextBoxes();
        void Close();
    }
}
