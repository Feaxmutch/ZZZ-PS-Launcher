using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ_PS_Launcher_2.WindowI
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
