namespace ZZZ_PS_Launcher
{
    internal interface ICreateProfileWindow
    {
        event Action<ProfileSettingName> ClickedSelect;
        event Action<ProfileSettingName> ClickedFromProfile;
        event Action ClickedSave;

        IPatches Patches { get; }

        void SetTextBox(ProfileSettingName name, string value);
        void SetTextBox(ProfileSettingName name, Patches patches);
        string GetTextBox(ProfileSettingName name);
        void ApplyFromTextBoxes();
        void Close();
    }
}
