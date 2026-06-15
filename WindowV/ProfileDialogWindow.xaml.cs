using System.Windows;

namespace ZZZ_PS_Launcher
{
    /// <summary>
    /// Логика взаимодействия для ProfileDialogWindow.xaml
    /// </summary>
    public partial class ProfileDialogWindow : Window
    {
        public ProfileDialogWindow()
        {
            InitializeComponent();
            Profile[] profiles = App.GetAllProfiles();

            foreach (var item in profiles)
            {
                ListBox_Profiles.Items.Add(item.Name);
            }
        }

        public string SelectedResult { get; private set; }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбрано ли что-то
            if (ListBox_Profiles.SelectedItem != null)
            {
                SelectedResult = ListBox_Profiles.SelectedItem.ToString();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент.");
            }
        }
    }
}
