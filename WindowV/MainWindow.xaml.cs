using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ZZZ_PS_Launcher;

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

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            Launching?.Invoke();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            WindowClosed?.Invoke();
        }
    }
}