using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ZZZ_PS_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        public event Action OpeningSettings;
        public event Action Launching;
        public event Action WindowClosed;

        public MainWindow()
        {
            InitializeComponent();
            InitializeElements();
            SetProgressLabel(string.Empty);
        }

        public void SetProgressLabel(string content)
        {
            if (content == string.Empty)
            {
                Label_LaunchProgress.SetValue(OpacityProperty, 0.0);
            }
            else
            {
                Label_LaunchProgress.SetValue(OpacityProperty, 1.0);
            }

            Label_LaunchProgress.Content = content;
        }

        private void OnButtonEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                DoubleAnimation anim = new(0.8, TimeSpan.FromSeconds(0.2));
                (sender as Button).BeginAnimation(OpacityProperty, anim);
            }
        }

        private void OnButtonLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                DoubleAnimation anim = new DoubleAnimation(1.0, TimeSpan.FromSeconds(0.2));
                (sender as Button).BeginAnimation(OpacityProperty, anim);
            }
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            OpeningSettings?.Invoke();
        }

        private async void Launch_Click(object sender, RoutedEventArgs e)
        {
            Launching?.Invoke();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            WindowClosed?.Invoke();
        }
    }
}