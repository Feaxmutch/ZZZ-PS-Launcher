using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ZZZ_PS_Launcher.WindowP;

namespace ZZZ_PS_Launcher
{
    public partial class MainWindow : Window
    {
        private void InitializeElements()
        {
            MainWindowP presenter = new(this);
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetBinding(Border.BackgroundProperty, new Binding("Background") { RelativeSource = RelativeSource.TemplatedParent });
            var contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetBinding(HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment") { RelativeSource = RelativeSource.TemplatedParent });
            contentPresenterFactory.SetBinding(VerticalAlignmentProperty, new Binding("VerticalContentAlignment") { RelativeSource = RelativeSource.TemplatedParent });
            contentPresenterFactory.SetBinding(MarginProperty, new Binding("Padding") { RelativeSource = RelativeSource.TemplatedParent });
            borderFactory.AppendChild(contentPresenterFactory);
            var universalTemplate = new ControlTemplate(typeof(Button)) { VisualTree = borderFactory };
            Button_Settings.Template = universalTemplate;
            Button_launch.Template = universalTemplate;
        }
    }
}
